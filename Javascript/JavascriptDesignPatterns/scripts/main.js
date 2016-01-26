/// <reference path="../typings/jquery/jquery.d.ts"/>
document.addEventListener('DOMContentLoaded', function () {
	octopus.init();
});

var octopus = {
	init: function() {
		model.init();
		viewCatList.render(model.catList);		
	},
	
	addNewCat: function(id) {
		var cat = model.getCatById(id);
		
		viewCatClick.renderNewCat(cat);
	},
	
	clickCat: function(ev) {
		var cat = model.getCatById(ev.target.title);
		
		cat.addClick();
		viewCatClick.updateClickCount(cat);
	},
	
	removeCat: function(catId) {
		viewCatClick.removeCat(catId);
	}
	

};

var viewCatList = {
	render: function(cats){
		var catList = $('#catList');
		var catHtml = $('#catElem').html();
		
		cats.forEach(function(v){
			var catElem = catHtml.replace('{{catImage}}', v.Image).replace(new RegExp('{{catId}}', 'g'), v.Id).replace('{{catName}}', v.Name);
			
			catList.append(catElem);
			document.getElementById(v.Id).addEventListener('change', viewCatList.onClick);
		});
	},
	
	onClick: function(ev){
		var chkCat = ev.target;
		
		if (chkCat.checked)
			octopus.addNewCat(chkCat.id);
		else
			octopus.removeCat(chkCat.id);
	}
};

var viewCatClick = {
	renderNewCat: function(cat){
		var html = $('#catDiv').html();
		html = html.replace(new RegExp('{{catId}}', 'g'), cat.Id)
				   .replace('{{catImage}}', cat.Image)
			       .replace('{{catName}}', cat.Name)
				   .replace('{{catClicks}}', cat.Clicks);
			
		$('article').append($(html));
		//$('article').on('click', '#img' + cat.Id, octopus.clickCat);
		$('#img' + cat.Id).click(octopus.clickCat);
	},
	
	updateClickCount: function(cat) {
		$('#div' + cat.Id).find('label').text(cat.Clicks);		
	},
	
	removeCat: function(catId){
		$('#div' + catId).remove();
	}
}

var Cat = function(id, name, image){
	this.Id     = id;
	this.Name   = name;
	this.Image  = image;
	this.Clicks = 0;
}

Cat.prototype.addClick = function(){
	this.Clicks++;
}

var model = {
	init: function(){

	},
	
	catList:[
			new Cat('cat1', 'Tom', 'images/cat.jpg'),
			new Cat('cat2', 'Brandy', 'images/cat2.jpg'),
			new Cat('cat3', 'Cat', 'images/cat3.jpg'),
			new Cat('cat4', 'Jerry', 'images/cat4.jpg'),
			new Cat('cat5', 'Flash', 'images/cat5.jpg')
		],
		
	getCatById: function(catId){
		return this.catList.filter(function(cat) { return cat.Id == catId;})[0];
	}
};