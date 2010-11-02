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


advBullet = Class.create();
advBullet.prototype = {
    /*
      Function: initialize
      Constructor. 
      
      Returns:
      A new advBullet
    */
    initialize: function(renderer) {
	this.renderer = renderer;
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
	var pos = {x: 20, y: 20};
	var inn_size = {width : this.size.width/1.5, height: this.size.height/1.5};
	var inn_pos = { x: pos.x + (this.size.width - inn_size.width)/2 ,
			y: pos.y + (this.size.height - inn_size.height)/2 };

	this.inner.setBounds(inn_pos.x, inn_pos.y, inn_size.width, inn_size.height);

	this.inner.setFill(inner_color);

	this.outer.setBounds(pos.x, pos.y, this.size.width, this.size.height);
	this.outer.setStroke({r: 255, g: 255, b: 255, w:5, a:0});

	this.renderer.add(this.inner);
	this.renderer.add(this.outer);
    }

}

/*
advDlgButton = Class.create();
advDlgButton.prototype = {
    initialize: function(renderer) {
	this.renderer = renderer;
	this.size = {width:50, height:50};

	this.outer = new Graphic.Circle(renderer);
	this.inner = new Graphic.Circle(renderer);
	
	return this;
    },

    render: function()
    {
	// var dlg_size = renderer.getSize();

	// 
	var inner_color = {r: 240, g: 161, b: 61, a:0}
	this.inner.setBounds(68.5,68.5, this.size.width/1.5, this.size.height/1.5);
	//	this.inner.setStroke();
	this.inner.setFill(inner_color);

	this.outer.setBounds(60,60, this.size.width, this.size.height);
	this.outer.setStroke({r: 255, g: 255, b: 255, w:5, a:0});

	this.renderer.add(this.inner);
	this.renderer.add(this.outer);
    }
}
*/

function adv_dialog(element, options)
{
    const stroke_w = 8;
    const renderer = get_renderer(element);
    
    var rect = new Graphic.Rectangle(renderer); 
    var width = parseInt($(element).getStyle("width"))-stroke_w*2;
    var height = parseInt($(element).getStyle("height"))-stroke_w*2;
    
    rect.setStroke({r: 255, g: 255, b: 255, w:stroke_w, a:0});
    rect.setBounds(stroke_w, stroke_w, width, height);
    rect.setFill({r: 44, g: 122, b: 199, a: 0});
    rect.setRoundCorner(20, 25);
    rect.setID("login")

    renderer.add(rect);

    title_bullet = new advBullet(renderer);
    title_bullet.render();
}