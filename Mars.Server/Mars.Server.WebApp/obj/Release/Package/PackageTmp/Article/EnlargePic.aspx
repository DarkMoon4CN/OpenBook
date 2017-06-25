<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnlargePic.aspx.cs" Inherits="Mars.Server.WebApp.Article.EnlargePic" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<title>图片详情</title>
<link href="/css/share-reset.css" rel="stylesheet" />
<link href="/css/share-app.css" rel="stylesheet" />
<script src="/js/plugin/jquery-1.9.1.min.js"></script>
<script src="/js/plugin/swiper.min.js"></script>
<link href="/css/swiper.css" rel="stylesheet" />
</head>
<body style="background-color:black !important;">

   <div class="swiper-container">
         <div class="swiper-wrapper">
	       <%=slideList%>
	    </div>
	</div>
        <input  type="hidden" id="index" value="<%=index %>"/>
   <script type="text/javascript">
            $(function () {
                var index = $("#index").val();
                var mySwiper = new Swiper('.swiper-container', {
                    direction: 'horizontal',
                    loop: false,
                    observer: true,
                    autoResize:false,
                    initialSlide: parseInt(index),
                    onInit: function(swiper){
                        var imgHeight = $("#imgeIndex_" + swiper.activeIndex).height();
                        var bodyHeight = $(document.body).outerHeight(false);
                        var bodyHeight = $(document).height();
                        var div_height = (bodyHeight - imgHeight) / 2;
                        $("#imgeIndex_" + swiper.activeIndex).parent().parent().parent().css("margin-top", div_height);
                    },
                    onSlideChangeStart: function (swiper) {
                        var imgHeight = $("#imgeIndex_" + swiper.activeIndex).height();
                        var bodyHeight = $(document).height();
                        var div_height = (bodyHeight - imgHeight) / 2;
                        $("#imgeIndex_" + swiper.activeIndex).parent().parent().parent().css("margin-top", div_height);
                    }
                })
            });

	</script>    
</body>
</html>

