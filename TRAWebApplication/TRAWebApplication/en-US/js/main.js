/* ------------------------------------- */
/* 1. Loading / Opening ................ */
/* ------------------------------------- */

$(window).on('load', function () {
	"use strict";

	setTimeout(function () {
		$(".loader").addClass('loaded');
	}, 2600);

	setTimeout(function () {
		$("#loading").addClass('loaded');
	}, 2800);

    /*setTimeout(function(){
        $(".brand-logo").removeClass('before-loading').addClass('loaded');
    },3000);

    setTimeout(function(){
        $("#fp-nav, .main-content").removeClass('before-loading').addClass('loaded');
    },3200);

    setTimeout(function(){
        $("footer").removeClass('before-loading').addClass('loaded');
    },3400);*/

	setTimeout(function () {
		$("#loading").remove();
		/*$(".brand-logo, footer").addClass('after-load');*/
	}, 3600);

    /*setTimeout(function(){
        $("#fp-nav").addClass('notransition');
    },4800);*/
});
$(document).ready(function () {
	"use strict";

	/* ------------------------------------- */
	/* 1. Text rotator on loading screen ... */
	/* ------------------------------------- */

	function dataWord() {

		$("[data-words]").attr("data-words", function (i, d) {
			var $self = $(this),
				$words = d.split("|"),
				tot = $words.length,
				c = 0;

			for (var loadtext = 0; loadtext < tot; loadtext++) { $self.append($('<span/>', { text: $words[loadtext] })); }

			$words = $self.find("span").hide();

			(function loop() {
				$self.animate({ width: $words.eq(c).width() });
				$words.stop().fadeOut().eq(c).fadeIn().delay(750).show(0, loop);
				c = ++c % tot;
			}());

		});

	}

	dataWord();


	/* ------------------------------------- */
	/* 2. Mega Menu ... */
	/* ------------------------------------- */

	//if you change this breakpoint in the style.css file (or _layout.scss if you use SASS), don't forget to update this value as well
	var MqL = 1170;
	//move nav element position according to window width
	moveNavigation();
	$(window).on('resize', function () {
		(!window.requestAnimationFrame) ? setTimeout(moveNavigation, 300) : window.requestAnimationFrame(moveNavigation);
	});

	//mobile - open lateral menu clicking on the menu icon
	$('.cd-nav-trigger').on('click', function (event) {
		event.preventDefault();
		if ($('.cd-main-content').hasClass('nav-is-visible')) {
			closeNav();
			$('.cd-overlay').removeClass('is-visible');
		} else {
			$(this).addClass('nav-is-visible');
			$('.cd-primary-nav').addClass('nav-is-visible');
			$('.cd-main-header').addClass('nav-is-visible');
			$('.cd-main-content').addClass('nav-is-visible').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
				$('body').addClass('overflow-hidden');
			});
			toggleSearch('close');
			$('.cd-overlay').addClass('is-visible');
		}
	});

	//open search form
	$('.cd-search-trigger').on('click', function (event) {
		event.preventDefault();
		toggleSearch();
		closeNav();
	});

	//close lateral menu on mobile 
	$('.cd-overlay').on('swiperight', function () {
		if ($('.cd-primary-nav').hasClass('nav-is-visible')) {
			closeNav();
			$('.cd-overlay').removeClass('is-visible');
		}
	});
	$('.nav-on-left .cd-overlay').on('swipeleft', function () {
		if ($('.cd-primary-nav').hasClass('nav-is-visible')) {
			closeNav();
			$('.cd-overlay').removeClass('is-visible');
		}
	});
	$('.cd-overlay').on('click', function () {
		closeNav();
		toggleSearch('close')
		$('.cd-overlay').removeClass('is-visible');
	});


	//prevent default clicking on direct children of .cd-primary-nav 
	$('.cd-primary-nav').children('.has-children').children('a').on('click', function (event) {
		event.preventDefault();
	});
	//open submenu
	$('.has-children').children('a').on('click', function (event) {
		if (!checkWindowWidth()) event.preventDefault();
		var selected = $(this);
		if (selected.next('ul').hasClass('is-hidden')) {
			//desktop version only
			selected.addClass('selected').next('ul').removeClass('is-hidden').end().parent('.has-children').parent('ul').addClass('moves-out');
			selected.parent('.has-children').siblings('.has-children').children('ul').addClass('is-hidden').end().children('a').removeClass('selected');
			$('.cd-overlay').addClass('is-visible');
		} else {
			selected.removeClass('selected').next('ul').addClass('is-hidden').end().parent('.has-children').parent('ul').removeClass('moves-out');
			$('.cd-overlay').removeClass('is-visible');
		}
		toggleSearch('close');
	});

	//submenu items - go back link
	$('.go-back').on('click', function () {
		$(this).parent('ul').addClass('is-hidden').parent('.has-children').parent('ul').removeClass('moves-out');
	});

	function closeNav() {
		$('.cd-nav-trigger').removeClass('nav-is-visible');
		$('.cd-main-header').removeClass('nav-is-visible');
		$('.cd-primary-nav').removeClass('nav-is-visible');
		$('.has-children ul').addClass('is-hidden');
		$('.has-children a').removeClass('selected');
		$('.moves-out').removeClass('moves-out');
		$('.cd-main-content').removeClass('nav-is-visible').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
			$('body').removeClass('overflow-hidden');
		});
	}

	function toggleSearch(type) {
		if (type == "close") {
			//close serach 
			$('.cd-search').removeClass('is-visible');
			$('.cd-search-trigger').removeClass('search-is-visible');
			$('.cd-overlay').removeClass('search-is-visible');
		} else {
			//toggle search visibility
			$('.cd-search').toggleClass('is-visible');
			$('.cd-search-trigger').toggleClass('search-is-visible');
			$('.cd-overlay').toggleClass('search-is-visible');
			if ($(window).width() > MqL && $('.cd-search').hasClass('is-visible')) $('.cd-search').find('input[type="search"]').focus();
			($('.cd-search').hasClass('is-visible')) ? $('.cd-overlay').addClass('is-visible') : $('.cd-overlay').removeClass('is-visible');
		}
	}

	function checkWindowWidth() {
		//check window width (scrollbar included)
		var e = window,
			a = 'inner';
		if (!('innerWidth' in window)) {
			a = 'client';
			e = document.documentElement || document.body;
		}
		if (e[a + 'Width'] >= MqL) {
			return true;
		} else {
			return false;
		}
	}

	function moveNavigation() {
		var navigation = $('.cd-nav');
		var desktop = checkWindowWidth();
		if (desktop) {
			navigation.detach();
			navigation.insertBefore('.cd-header-buttons');
		} else {
			navigation.detach();
			navigation.insertAfter('.cd-main-content');
		}
	}

	/* ------------------------------------- */
	/* 2.Carousel Slider ... */
	/* ------------------------------------- */
	// Home Awareness Carousel
	if ($('.awareness-carousel').length) {
		$('.awareness-carousel').owlCarousel({
			//center: true,
			//autoWidth: true,
			loop: false,
			rewind: true,
			margin: 30,
			nav: true,
			dots: false,
			smartSpeed: 500,
			animateIn: true,
			autoplay: true,
			autoplayTimeout: 5000,
			lazyLoad: true,
			autoplayHoverPause: true,
			navText: ['<span class="icon flaticon-left-arrow-1"></span>', '<span class="icon flaticon-right-arrow-1"></span>'],
			//responsiveBaseElement: ".box-to-check",
			responsive: {
				0: {
					items: 1
				},
				480: {
					items: 1
				},
				600: {
					items: 2,
					margin: 15
				},
				768: {
					items: 2
				},
				992: {
					items: 2
				},
				1200: {
					items: 3
				}
			}
		});
	}

	
	$(".input-group.date").find('input[type=text]').on("change", function () {
		if (this.value != "") {
			try {
				$(this).parent().addClass("input--filled");
				$(this).attr('value', this.value);
			}
			catch(ex)
			{ }
		}
	});
	
	$('.selectorautocomplete').on("focus", function () {
		if ($(this).parent().css('z-index') < 3) {
			$(this).parent().css({'z-index':'3'});
		}
		var index = $(this).parent().eq(0).css('z-index');
		$(this).parent().eq(0).each(function() {
			$(this).css('z-index', ++index);
		});
	});

	$('.msform input[type="text"]').on("change", function () {
		if (this.value != "") {
			try {
				$(this).tooltipster('destroy');
			}
			catch(ex)
			{ }
		}
	});
	$('.msform input[type="text"]').on("keyup", function () {
		if (this.value != "") {
			try {
				$(this).tooltipster('destroy');
			}
			catch(ex)
			{ }
		}
	});
});


 