// adv-dlgbox.js

function get_renderer(element)
{
    var renderer;

    if (Graphic.rendererSupported("VML"))
	renderer = new Graphic.VMLRenderer(element); 
    else if (Graphic.rendererSupported("SVG"))
	renderer = new Graphic.SVGRenderer(element);       
    else if (Graphic.rendererSupported("Canvas"))
	renderer = new Graphic.CanvasRenderer(element);

    return renderer;
}



advFrame = Class.create( {
    /*
      Function: initialize
      Constructor. 
      
      Returns:
      A new advFrame
    */
    initialize: function(renderer, stroke, bgcolor) {
	this.renderer = renderer;
	this.size = renderer.getSize();

	this.stroke = stroke;
	this.bgcolor = bgcolor;

	this.rect = new Graphic.Rectangle(renderer);
	
	return this;
    },

    /*
      Function: render      
      
      adds frame to renderer
      
    */  
    render: function()
    {
	const stroke_w = this.stroke.w;
	const padding = 20;
    
	var rect = this.rect;
	var width = this.size.width - stroke_w*2 - padding*2;
	var height = this.size.height - stroke_w*2 - padding*2;
    
	rect.setStroke(this.stroke);
	rect.setBounds(stroke_w + padding, stroke_w + padding, width, height);
	//rect.setFill({r: 44, g: 122, b: 199, a: 0});
	rect.setRoundCorner(20, 25);
	rect.setID("login")

	this.renderer.add(rect);
    }
}); // advFrame


advBullet = Class.create( {
    /*
      Function: initialize
      Constructor. 
      
      Returns:      A new advBullet
    */
    initialize: function(renderer, stroke, bgcolor) {
	this.renderer = renderer;
	this.stroke = stroke;
	this.bgcolor = bgcolor;

	this.size = {width:50, height:50};
	this.outer = new Graphic.Circle(renderer);
	this.inner = new Graphic.Circle(renderer);

	return this;
    },
    /*
      Function: render      
      
      adds bullet to renderer
      
    */  
    render: function()
    {
	// var dlg_size = renderer.getSize();

	// 
	var inner_color = {r: 240, g: 161, b: 61, a:0}
	var pos = {x: 75, y: 4};
	var inn_size = {width : this.size.width/1.5, height: this.size.height/1.5};
	var inn_pos = { x: pos.x + (this.size.width - inn_size.width)/2 ,
			y: pos.y + (this.size.height - inn_size.height)/2 };

	this.inner.setBounds(inn_pos.x, inn_pos.y, inn_size.width, inn_size.height);
	this.inner.setFill(inner_color);

	this.outer.setBounds(pos.x, pos.y, this.size.width, this.size.height);
	this.outer.setFill(this.bgcolor);
	this.outer.setStroke(this.stroke);

	this.renderer.add(this.outer);
	this.renderer.add(this.inner);
    }

}); // advBullet

advDlgBox =  Class.create( {
    /* Constructor
     */
    initialize : function(element, options)    {
	this.element = element;
	this.renderer = get_renderer(element);

	this.stroke =  {r: 255, g: 255, b: 255, w:6, a:0};
	this.bgcolor = {r:44, g:122 , b:199};

	this.frame = new advFrame(this.renderer, this.stroke, this.bgcolor);
	this.title = $$('#'+element.id + ' h2')[0];
	this.titleBullet = new advBullet(this.renderer, this.stroke, this.bgcolor);

	this.position_title();

	buttons = element.getElementsBySelector('div.button');
	for (i=0; i<buttons.length; i++)
	    alert(buttons[i].title);

	this.frame.render();
	this.titleBullet.render();
    },

    position_title : function() {
	this.title.setStyle({'top':'-10px', 'left':'127.5px', 'padding':'0px 5px 0px 12px'});
    }    

}); // advDlgBox



function init_dialogs()
{
    var boxes = $$('div.dlgbox');

    for (i =0; i<boxes.length; i++) 
	new advDlgBox(boxes[i]);
}

