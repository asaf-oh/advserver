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

  # GET /accounts/login
  # GET /accounts/login.xml
  def login
    # @account = Account.new
    I18n.locale = 'he'
    respond_to do |format|      
      format.html { render :action => "login" }
      format.xml  { render :xml => @account }
    end
  end

  # POST /accounts/create
  # POST /accounts/create.xml
  def create
    printf("create : %s/%s \n", params["name"], params["password"])
    @account = Account.new(:name => params["name"], :email => params["email"], :password => params["password"])
    
    respond_to do |format|
      if @account.save
        printf("save OK\n")
        format.html { redirect_to(@account, :notice => 'Account was successfully created.') }
        format.xml  { render :xml => @account, :status => :created, :location => @account }
      else
        printf("save FAIL\n")
        format.html { render :action => "new" }
        format.xml  { render :xml => @account.errors, :status => :unprocessable_entity }
      end
    end
  end

  # GET /accounts/show
  # GET /accounts/show.xml
  def show
    printf("show : name %s", params["id"]);
    respond_to do |format|      
      format.html { render :action => "new" }
      format.xml  { render :xml => @account }
    end
  end
end
