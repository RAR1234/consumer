var current_fs, next_fs, previous_fs; //fieldsets
var left, opacity, scale; //fieldset properties which we will animate
var animating; //flag to prevent quick multi-click glitches
function validateinputs(cntrl) {
	var valid = true;
	cntrl.find("*[mandatory]:visible").each(function () {
		//debugger;
		if ($(this)[0].type === 'radio' || $(this)[0].type === 'checkbox') {
			if (!$("input[name='" + $(this)[0].name + "']:checked").prop("checked")) {
				$(this).tooltipster({
					theme: 'tooltipster-shadow',
					content: '<font size="2" color="#cf152d">' + (($(this)[0].type === 'checkbox') ? 'يرجى تحديد هذا المربع للمتابعة!' : 'ارجوك اختر خيار واحد!') + '</font>',
					contentAsHTML: true,
					side: 'right',
					maxWidth: 300,
					trigger: 'custom'
				});
				$(this).tooltipster('open');
				$(this).focus();
				valid = false;
				return false;
			}
			else {
				try {
					$(this).tooltipster('destroy');
				}
				catch (ex) { }
			}
		}
		else
			if ($(this).hasClass('dropzone')) {

				if ($(this).hasClass('dz-max-files-reached') === false && $(this)[0].dropzone.options.maxFiles === 1) {

					$(this).tooltipster({
						theme: 'tooltipster-shadow',
						content: '<font size="2" color="#cf152d">الرجاء ملء هذه الخانة!</font>',
						contentAsHTML: true,
						side: 'bottom',
						maxWidth: 300,
						trigger: 'custom'
					});
					$(this).tooltipster('open');
					$(this).focus();
					valid = false;
					$([document.documentElement, document.body]).animate({
						scrollTop: $(this).offset().top
					}, 0);
					return false;
				} else {
					try {
						$(this).tooltipster('destroy');
					}
					catch (ex) { }
				}

				if ($(this)[0].dropzone.options.maxFiles === 10) {
				
					var multiplesFiles = $(this);
					for (var i = 0; i < multiplesFiles[0].dropzone.files.length; i++) {

						if (multiplesFiles[0].dropzone.files.length > 0) {

							if (multiplesFiles[0].innerText.includes("File is too big") || multiplesFiles[0].innerText.includes("You can't upload files of this type")) {
							
								$(this).tooltipster({
									theme: 'tooltipster-shadow',
									content: '<font size="2" color="#cf152d">نوع الملف أو حجم الملف غير صالح (بحد أقصى 2 ميغا بايت)</font>',
									contentAsHTML: true,
									side: 'bottom',
									maxWidth: 300,
									trigger: 'custom'
								});
								$(this).tooltipster('open');
								$(this).focus();
								valid = false;

								$([document.documentElement, document.body]).animate({
									scrollTop: $(this).offset().top
								}, 0);
								return false;
							}
							else {
								try {
									$(this).tooltipster('destroy');
								}
								catch (ex) { }
							}

						}
					}
				}

			}



			else {

				if ($(this)[0].value === "" || $(this)[0].value === ".د.ب  0.00") {
					$(this).tooltipster({
						theme: 'tooltipster-shadow',
						content: '<font size="2" color="#cf152d">الرجاء ملء هذه الخانة!</font>',
						contentAsHTML: true,
						side: 'bottom',
						maxWidth: 300,
						trigger: 'custom'
					});
					$(this).tooltipster('open');
					$(this).focus();
					valid = false;
					return false;
				}
				else {
					try {
						$(this).tooltipster('destroy');
					}
					catch (ex) { }
				}
			}
	});
	if (valid) {
		cntrl.find("*[number]:visible").each(function () {
			if ($(this)[0].value != "") {
				if (isNaN($(this)[0].value)) {
					$(this).tooltipster({
						theme: 'tooltipster-shadow',
						content: '<font size="2" color="red">الأرقام مقبولة فقط!</font>',
						contentAsHTML: true,
						side: 'bottom',
						maxWidth: 300,
						trigger: 'custom'
					});
					$(this).tooltipster('open');
					$(this).focus();
					valid = false;
					return false;
				}
			}
			else {
				try {
					$(this).tooltipster('destroy');
				}
				catch (ex) { }
			}
		});
	}
	if (valid) {
		var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
		cntrl.find("*[email]:visible").each(function () {
			if ($(this)[0].value != "") {
				if (!emailReg.test($(this)[0].value)) {
					$(this).tooltipster({
						theme: 'tooltipster-shadow',
						content: '<font size="2" color="red">Please enter valid eMail ID!</font>',
						contentAsHTML: true,
						side: 'bottom',
						maxWidth: 300,
						trigger: 'custom'
					});
					$(this).tooltipster('open');
					$(this).focus();
					valid = false;
					return false;
				}
			}
			else {
				try {
					$(this).tooltipster('destroy');
				}
				catch (ex) { }
			}
		});
	}
	return valid;
}
$(".next").click(function (e) {
	var valid = true;
	current_fs = $(this).parent().parent();
	next_fs = $(this).parent().parent().next();
	valid = validateinputs(current_fs);
	if (valid) {
		if (animating) return false;
		animating = true;
		//activate next step on progressbar using the index of next_fs
		$("#progressbarind li").eq($(".individual-set fieldset").index(next_fs)).addClass("active");
		$("#progressbarcomp li").eq($(".company-set fieldset").index(next_fs)).addClass("active");
		$("#progressbargov li").eq($(".government-set fieldset").index(next_fs)).addClass("active");

		$("#progressbarcomplaint li").eq($(".complaint-set fieldset").index(next_fs)).addClass("active");
		$("#progressbarenquiry li").eq($(".enquiry-set fieldset").index(next_fs)).addClass("active");
		$("#progressbarsuggestion li").eq($(".suggestion-set fieldset").index(next_fs)).addClass("active");

		//show the next fieldset
		next_fs.show();
		//hide the current fieldset with style
		current_fs.animate({ opacity: 0 }, {
			step: function (now) {
				//as the opacity of current_fs reduces to 0 - stored in "now"
				//1. scale current_fs down to 80%
				scale = 1 - (1 - now) * 0.2;
				//2. bring next_fs from the right(50%)
				left = (now * 50) + "%";
				//3. increase opacity of next_fs to 1 as it moves in
				opacity = 1 - now;
				current_fs.css({
					'transform': 'scale(' + scale + ')',
					'position': 'absolute',
					'top': '0'
				});
				next_fs.css({ 'left': left, 'opacity': opacity, 'position': 'relative', 'top': '0' });
			},
			duration: 800,
			complete: function () {
				current_fs.hide();
				animating = false;
			},
			//this comes from the custom easing plugin
			easing: 'easeInOutBack'
		});
	}
	else {
		return false;
	}
});

$(".previous").click(function () {
	if (animating) return false;
	animating = true;

	current_fs = $(this).parent().parent();
	previous_fs = $(this).parent().parent().prev();

	current_fs.find("input[type=text]").each(function () {
		try {
			$(this).tooltipster('destroy');
		}
		catch (ex) { }
	});

	//de-activate current step on progressbar
	$("#progressbarind li").eq($(".individual-set fieldset").index(current_fs)).removeClass("active");
	$("#progressbarcomp li").eq($(".company-set fieldset").index(current_fs)).removeClass("active");
	$("#progressbargov li").eq($(".government-set fieldset").index(current_fs)).removeClass("active");

	$("#progressbarcomplaint li").eq($(".complaint-set fieldset").index(current_fs)).removeClass("active");
	$("#progressbarenquiry li").eq($(".enquiry-set fieldset").index(current_fs)).removeClass("active");
	$("#progressbarsuggestion li").eq($(".suggestion-set fieldset").index(current_fs)).removeClass("active");

	//show the previous fieldset
	previous_fs.show();
	//hide the current fieldset with style
	current_fs.animate({ opacity: 0 }, {
		step: function (now) {
			//as the opacity of current_fs reduces to 0 - stored in "now"
			//1. scale previous_fs from 80% to 100%
			scale = 0.8 + (1 - now) * 0.2;
			//2. take current_fs to the right(50%) - from 0%
			left = ((1 - now) * 50) + "%";
			//3. increase opacity of previous_fs to 1 as it moves in
			opacity = 1 - now;
			current_fs.css({ 'left': left });
			previous_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
		},
		duration: 800,
		complete: function () {
			current_fs.hide();
			current_fs.css({ 'position': 'absolute', 'top': '0' });
			previous_fs.css({ 'position': 'relative' });
			animating = false;
		},
		//this comes from the custom easing plugin
		easing: 'easeInOutBack'
	});
	if (previous_fs.length == 0) {
		$(".fset1").animate({ opacity: 'initial' }, {
			duration: 800,
			complete: function () {
				$(".fset1").show();
				current_fs.hide();
				current_fs.css({ 'position': 'relative', 'top': '0' });
				$(".progressbar").hide();
				$(".main-title").hide();
				animating = false;
			},
			//this comes from the custom easing plugin
			easing: 'easeInOutBack'
		});
	}
});

/*Dropzone Init*/
/*$("#remove_link").dropzone({
	addRemoveLinks: true,
});*/

/*Registration Submit Button*/
$(".submit").click(function () {
	current_fs = $(this).parent().parent();
	if (validateinputs(current_fs)) {
		Swal.fire({
			icon: 'success',
			title: 'شكرا لكم!',
			confirmButtonText: 'دخول',
			showCancelButton: true,
			cancelButtonText: 'الصفحة الرئيسية',
			allowOutsideClick: false,
			allowEscapeKey: false,
			html:
				'<p>المستخدم مسجل بنجاح.</p>' +
				'<p>يرجى التحقق من بريدك الإلكتروني المسجل للتأكيد.</p>'
		}).then(function (result) {
			//debugger;
			if (result.dismiss == 'cancel') {
				window.location.href = "register.html";
			}
			else {
				window.location.href = "index.html";
			}
		})
	}
	return false;
});

/*Case Submit Button*/
$(".submitcase").click(function () {
	current_fs = $(this).parent().parent();
	if (validateinputs(current_fs)) {
		Swal.fire({
			icon: 'success',
			title: 'شكرا لكم!',
			confirmButtonText: 'الخروج',
			showCancelButton: true,
			cancelButtonText: 'لوحة القيادة',
			allowOutsideClick: false,
			allowEscapeKey: false,
			html:
				'<p>تم تقديم قضيتك بنجاح.</p>' +
				'<p>الرقم المرجعي:  <b style="color:#D2152D">CAS-08308-H2C4X2</b></p>'
		}).then(function (result) {
			//debugger;
			if (result.dismiss == 'cancel') {
				window.location.href = "dashboard.html";
			}
			else {
				window.location.href = "index.html";
			}
		})
	}
	return false;
});

$(".submitPwd").click(function () {
	current_fs = $(this).parent().parent();
	if (validateinputs(current_fs)) {
		Swal.fire({
			icon: 'success',
			title: 'نجاح!',
			confirmButtonText: 'حسنا',
			showCancelButton: true,
			cancelButtonText: 'لوحة القيادة',
			//allowOutsideClick: false,
			//allowEscapeKey: false,
			html:
				'<p>لقد تم تحديث كلمة السر الخاصة بك بنجاح. شكرا لكم.</p>'
		}).then(function (result) {
			//debugger;
			if (result.dismiss == 'cancel') {
				window.location.href = "dashboard.html";
			}
			else {
				window.location.href = "my-profile.html";
			}
		})
	}
	return false;
});

$(".submitProfile").click(function () {
	current_fs = $(this).parent().parent();
	if (validateinputs(current_fs)) {
		Swal.fire({
			icon: 'success',
			title: 'نجاح!',
			confirmButtonText: 'حسنا',
			showCancelButton: true,
			cancelButtonText: 'لوحة القيادة',
			//allowOutsideClick: false,
			//allowEscapeKey: false,
			html:
				'<p>تم تحديث ملفك الشخصي بنجاح. شكرا لكم.</p>'
		}).then(function (result) {
			//debugger;
			if (result.dismiss == 'cancel') {
				window.location.href = "dashboard.html";
			}
			else {
				window.location.href = "my-profile.html";
			}
		})
	}
	return false;
});

$(".submitFeedback").click(function () {
	current_fs = $(this).parent().parent();
	if (validateinputs(current_fs)) {
		Swal.fire({
			icon: 'success',
			title: 'نجاح!',
			confirmButtonText: 'حسنا',
			showCancelButton: true,
			cancelButtonText: 'لوحة القيادة',
			//allowOutsideClick: false,
			//allowEscapeKey: false,
			html:
				'<p>تم ارسال ملاحظاتك بنجاح. شكرا لكم.</p>'
		}).then(function (result) {
			//debugger;
			if (result.dismiss == 'cancel') {
				window.location.href = "dashboard.html";
			}
			else {
				window.location.href = "case-details.html";
			}
		})
	}
	return false;
});

$(".postReply").click(function () {
	current_fs = $(this).parent().parent();
	if (validateinputs(current_fs)) {
		Swal.fire({
			icon: 'success',
			title: 'نجاح!',
			confirmButtonText: 'حسنا',
			showCancelButton: true,
			cancelButtonText: 'لوحة القيادة',
			//allowOutsideClick: false,
			//allowEscapeKey: false,
			html:
				'<p>تم إرسال ردك بنجاح. شكرا لكم.</p>'
		}).then(function (result) {
			//debugger;
			if (result.dismiss == 'cancel') {
				window.location.href = "dashboard.html";
			}
			else {
				window.location.href = "case-details.html";
			}
		})
	}
	return false;
});
