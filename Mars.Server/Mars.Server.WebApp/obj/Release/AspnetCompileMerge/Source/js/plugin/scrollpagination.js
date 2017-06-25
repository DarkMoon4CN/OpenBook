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
        return {//�ƶ��ն�������汾��Ϣ 
            trident: u.indexOf('Trident') > -1, //IE�ں�
            presto: u.indexOf('Presto') > -1, //opera�ں�
            webKit: u.indexOf('AppleWebKit') > -1, //ƻ�����ȸ��ں�
            gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //����ں�
            mobile: !!u.match(/AppleWebKit.*Mobile.*/) || !!u.match(/AppleWebKit/), //�Ƿ�Ϊ�ƶ��ն�
            ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios�ն�
            android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android�ն˻���uc�����
            iPhone: u.indexOf('iPhone') > -1 || u.indexOf('Mac') > -1, //�Ƿ�ΪiPhone����QQHD�����
            iPad: u.indexOf('iPad') > -1, //�Ƿ�iPad
            webApp: u.indexOf('Safari') == -1 //�Ƿ�webӦ�ó���û��ͷ����ײ�
        };
    }(),
    language: (navigator.browserLanguage || navigator.language).toLowerCase()
};