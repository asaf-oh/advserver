// ***********************************************************************
// Copyright (c) 2006 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Threading;
using NUnit.Framework.Api;

namespace NUnit.Framework.Internal
{
    /// <summary>
    /// The EventPumpState enum represents the state of an
    /// EventPump.
    /// </summary>
    public enum EventPumpState
    {
        /// <summary>
        /// The pump is stopped
        /// </summary>
        Stopped,

        /// <summary>
        /// The pump is pumping events with no stop requested
        /// </summary>
        Pumping,

        /// <summary>
        /// The pump is pumping events but a stop has been requested
        /// </summary>
        Stopping
    }

	/// <summary>
	/// EventPump pulls events out of an EventQueue and sends
	/// them to a listener. It is used to send events back to
	/// the client without using the CallContext of the test
	/// runner thread.
	/// </summary>
	public class EventPump : IDisposable
	{
		#region Instance Variables
		/// <summary>
		/// The downstream listener to which we send events
		/// </summary>
		ITestListener listener;
		
		/// <summary>
		/// The queue that holds our events
		/// </summary>
		EventQueue events;
		
		/// <summary>
		/// Thread to do the pumping
		/// </summary>
		Thread pumpThread;

		/// <summary>
		/// The current state of the eventpump
		/// </summary>
		EventPumpState pumpState = EventPumpState.Stopped;

		/// <summary>
		/// If true, stop after all tests are finished
		/// </summary>
		private bool autostop;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="eventListener">The EventListener to receive events</param>
		/// <param name="events">The event queue to pull events from</param>
		/// <param name="autostop">Set to true to stop pump after all tests finish</param>
		public EventPump( ITestListener eventListener, EventQueue events, bool autostop)
		{
			this.listener = eventListener;
			this.events = events;
			this.autostop = autostop;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The current state of the pump
		/// </summary>
		public EventPumpState PumpState
		{
			get { return pumpState; }
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Dispose stops the pump
		/// </summary>
		public void Dispose()
		{
			Stop();
		}

		/// <summary>
		/// Start the pump
		/// </summary>
		public void Start()
		{
			if ( pumpState == EventPumpState.Stopped )  // Ignore if already started
			{
				this.pumpThread = new Thread( new ThreadStart( PumpThreadProc ) );
				this.pumpThread.Name = "EventPumpThread";
                pumpState = EventPumpState.Pumping;
                this.pumpThread.Start();
			}
		}

		/// <summary>
		/// Tell the pump to stop after emptying the queue.
		/// </summary>
		public void Stop()
		{
			if ( pumpState == EventPumpState.Pumping ) // Ignore extra calls
			{
				lock( events )
				{
					pumpState = EventPumpState.Stopping;
					Monitor.Pulse( events ); // In case thread is waiting
				}
				this.pumpThread.Join();
			}
		}
		#endregion

		#region PumpThreadProc
		/// <summary>
		/// Our thread proc for removing items from the event
		/// queue and sending them on. Note that this would
		/// need to do more locking if any other thread were
		/// removing events from the queue.
		/// </summary>
		private void PumpThreadProc()
		{
			ITestListener hostListeners = CoreExtensions.Host.Listeners;
#if NET_4_0
            bool lockTaken = false;
			Monitor.Enter( events, ref lockTaken );
#else
			Monitor.Enter( events );
#endif
            try
            {
                int pendingTests = 0;

                while (this.events.Count > 0 || pumpState == EventPumpState.Pumping)
                {
                    while (this.events.Count > 0)
                    {
                        Event e = this.events.Dequeue();
                        e.Send(this.listener);
						e.Send(hostListeners);
                        if (autostop)
                        {
                            if (e is TestStartedEvent)
                                pendingTests++;
                            else if (e is TestFinishedEvent && --pendingTests == 0)
                                pumpState = EventPumpState.Stopping;
                        }
                    }
                    // Will be pulsed if there are any events added
                    // or if it's time to stop the pump.
                    if ( pumpState != EventPumpState.Stopping )
                        Monitor.Wait(events);
                }
            }
            catch (Exception ex)
            {
                InternalTrace.Error("Exception in pump thread", ex);
            }
			finally
			{
#if NET_4_0
                if (lockTaken)
#endif
				    Monitor.Exit( events );
                pumpState = EventPumpState.Stopped;
				//pumpThread = null;
			}
		}
		#endregion
	}
}
