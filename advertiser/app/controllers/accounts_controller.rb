class AccountsController < ApplicationController


  # GET /accounts/new
  # GET /accounts/new.xml
  def new
    # @account = Account.new
    respond_to do |format|      
      format.html { render :action => "new" }
      format.xml  { render :xml => @account }
    end
  end

  # POST /accounts/create
  # POST /accounts/create.xml
  def create
    printf("create")
    @account = Account.new(params[:account])
    
    respond_to do |format|
      if @account.save
        format.html { redirect_to(@account, :notice => 'Account was successfully created.') }
        format.xml  { render :xml => @account, :status => :created, :location => @account }
      else
        format.html { render :action => "new" }
        format.xml  { render :xml => @account.errors, :status => :unprocessable_entity }
      end
    end
  end

  # GET /accounts/show
  # GET /accounts/show.xml
  def show
    printf("show!");
  end
end
