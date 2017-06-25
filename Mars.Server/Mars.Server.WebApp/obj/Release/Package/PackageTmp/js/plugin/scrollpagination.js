/*
**	Anderson Ferminiano
**	contato@andersonferminiano.com -- feel free to contact me for bugs or new implementations.
**	jQuery ScrollPagination
**	28th/March/2011
**	http://andersonferminiano.com/jqueryscrollpagination/
**	You may use this script for free, but keep my credits.
**	Thank you.
*/

(function( $ ){
	 
    //var hasmoredata = true;
		 
 $.fn.scrollPagination = function(options) {
  	
		var opts = $.extend($.fn.scrollPagination.defaults, options);  
		var target = opts.scrollTarget;
		if (target == null){
			target = obj; 
	 	}
		opts.scrollTarget = target;
		$.fn.scrollPagination.hadmoredata = true;
		return this.each(function() {
		  $.fn.scrollPagination.init($(this), opts);
		});

  };
  
  $.fn.stopScrollPagination = function(){
	  return this.each(function() {
	 	$(this).attr('scrollPagination', 'disabled');
	  });
	  
  };
  $.fn.scrollPagination.hadmoredata = true;

  $.fn.scrollNoData = function ()
  {
      $.fn.scrollPagination.hadmoredata = false;
  }

  $.fn.scrollPagination.loadContent = function(obj, opts){
    var target = opts.scrollTarget;
    var mayLoadContent = opts.isLoadDataNow;
    if (!opts.isLoadDataNow) {
        if ($(target).height() == 0 && browser.versions.android) {
            mayLoadContent = true;
        } else {
            mayLoadContent = $.fn.scrollPagination.hadmoredata && $(target).scrollTop() + opts.heightOffset >= $(document).height() - $(target).height();
        }
    } 

	 if (mayLoadContent){
		 if (opts.beforeLoad != null){
			opts.beforeLoad(); 
		 }
        $(obj).attr('scrollPagination', 'disabled');
		 //$(obj).children().attr('rel', 'loaded');
		 $.ajax({
			  type: (opts.httpType==0 ? 'GET':'POST'),
			  url: opts.contentPage,
			  data: opts.contentData,
			  success: function(data){
				//$(obj).append(data); 
//				var objectsRendered = $(obj).children('[rel!=loaded]');
//				
				if (opts.afterLoad != null){
				    //					opts.afterLoad(objectsRendered);	
				    opts.afterLoad(data);
				}

				//if (data.list.length = 0)
				//{
				//    $.fn.scrollPagination.hadmoredata = false;
				//}

				$(obj).attr('scrollPagination', 'enabled');
			  },
			  dataType: 'html'
		 });
	 }
	 
  };
  
  $.fn.scrollPagination.init = function(obj, opts){
	 var target = opts.scrollTarget;
	 $(obj).attr('scrollPagination', 'enabled');
	
	 $(target).scroll(function(event){
		if ($(obj).attr('scrollPagination') == 'enabled'){
	 		$.fn.scrollPagination.loadContent(obj, opts);		
		}
		else {
			event.stopPropagation();	
		}
	 });
	 
	 $.fn.scrollPagination.loadContent(obj, opts);
	 
 };
	
 $.fn.scrollPagination.defaults = {
      	 'contentPage' : null,
     	 'contentData' : {},
		 'beforeLoad': null,
		 'afterLoad': null	,
		 'scrollTarget': null,
		 'heightOffset': 0,
		 'httpType':0
 };	
})(jQuery);

var browser = {
    versions: function () {
        var u = navigator.userAgent, app = navigator.appVersion;
        return {//移动终端浏览器版本信息 
            trident: u.indexOf('Trident') > -1, //IE内核
            presto: u.indexOf('Presto') > -1, //opera内核
            webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
            gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
            mobile: !!u.match(/AppleWebKit.*Mobile.*/) || !!u.match(/AppleWebKit/), //是否为移动终端
            ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
            android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或者uc浏览器
            iPhone: u.indexOf('iPhone') > -1 || u.indexOf('Mac') > -1, //是否为iPhone或者QQHD浏览器
            iPad: u.indexOf('iPad') > -1, //是否iPad
            webApp: u.indexOf('Safari') == -1 //是否web应该程序，没有头部与底部
        };
    }(),
    language: (navigator.browserLanguage || navigator.language).toLowerCase()
};