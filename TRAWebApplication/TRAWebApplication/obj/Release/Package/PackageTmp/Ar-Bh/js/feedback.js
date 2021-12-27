var $canvas = $('body');
var $eyes = $('.eye');
var $rateInputs = $('.rate-input');
var $rateInputs1 = $('.rate-input1');
var $rateInputs2 = $('.rate-input2');
var $rateInputs3 = $('.rate-input3');
var $rateInputs4 = $('.rate-input4');

function vendorize(key, value) {
  var vendors = ['webkit', 'moz', 'ms', 'o', ''];
  var result = {};

  vendors.map(function (vendor) {
    var vKey = vendor ? '-' + vendor + '-' + key : key;

    result[vKey] = value;
  });

  return result;
}

//https://github.com/jfmdev/jqEye/blob/master/Source/jqeye.js
function circle_position(x, y, r) {
  // Circle: x^2 + y^2 = r^2
  var res = { x: x, y: y };
  if (x * x + y * y > r * r) {
    if (x !== 0) {
      var m = y / x;
      res.x = Math.sqrt(r * r / (m * m + 1));
      res.x = x > 0 ? res.x : -res.x;
      res.y = Math.abs(m * res.x);
      res.y = y > 0 ? res.y : -res.y;
    } else {
      res.y = y > 0 ? r : -r;
    }
  }
  return res;
};

function findCenter(coords, sizeX, sizeY) {
  return {
    x: coords.left + sizeX / 2,
    y: coords.top + sizeY / 2
  };
}

function changeEyesPosition(px, py) {
  function changePosition() {
    var $t = $(this);
    var t_w = $t.width();
    var t_h = $t.height();
    var t_o = $t.offset();
    var t_p = $t.position();
    var abs_center = findCenter(t_o, t_w, t_h);
    var pos_x = px - abs_center.x + $(window).scrollLeft();
    var pos_y = py - abs_center.y + $(window).scrollTop();
    var cir = circle_position(pos_x, pos_y, t_w / 8);
    var styles = vendorize('transform', 'translateX(' + cir.x + 'px) translateY(' + cir.y + 'px)');

    $(this).find('.pupil').css(styles);
  }

  $eyes.each(changePosition);
}

function handleMouseMove(e) {
  var px = e.pageX,
      py = e.pageY;

  changeEyesPosition(px, py);
}

$canvas.on('mousemove', handleMouseMove);

function getFace($element) {
  return $element.parent('.face-wrapper').find('.face');
}

function handleFaceHover($face, index) {
    //var $hint = $element.parent('.div').last().find('.faces-hint');
    console.log($('.set' + index));
    var $hint = $('.set' + index).find('.faces-hint');
  var hintText = $face.attr('data-hint') || $hint.attr('data-default-hint');
  $hint.text(hintText);
}

function handleFacesHover(e,index) {
  var $face = getFace($(e.target));

    handleFaceHover($face, index);
}

$('.set1').on('mousemove', function (e) { handleFacesHover(e, '1') });
$('.set2').on('mousemove', function (e) {handleFacesHover(e, '2') });
$('.set3').on('mousemove', function (e) {handleFacesHover(e, '3') });
$('.set4').on('mousemove', function (e) {handleFacesHover(e, '4') });

function handleFeedbackTitleHover(e) {
  var isHover = e.type === 'mouseenter';
  $(this).parent().toggleClass('title-hovered', isHover);
}

$('.feedback-title').on('mouseenter mouseleave', handleFeedbackTitleHover);

function handleFeedbackToggle() {
  var $this = $(this),
      $parent = $this.parent();

  $parent.toggleClass('at-bottom');

  $parent.find('.face-wrapper').each(function (index) {
    setTimeout(function (face) {
      face.toggleClass('slide-out-y-alt', $parent.hasClass('at-bottom'));
    }, index * 60, $(this));
  });
}
$('.feedback-title').on('click', handleFeedbackToggle);

/*Function RateInput*/
function handleRateInputChange() {
  getFace($rateInputs).addClass('grayscale');
  getFace($(this)).removeClass('grayscale');
}
$rateInputs.on('change', handleRateInputChange);

/*Function RateInput1*/
function handleRateInputChange1() {
  getFace($rateInputs1).addClass('grayscale');
  getFace($(this)).removeClass('grayscale');
}
$rateInputs1.on('change', handleRateInputChange1);

/*Function RateInput2*/
function handleRateInputChange2() {
  getFace($rateInputs2).addClass('grayscale');
  getFace($(this)).removeClass('grayscale');
}
$rateInputs2.on('change', handleRateInputChange2);

/*Function RateInput3*/
function handleRateInputChange3() {
  getFace($rateInputs3).addClass('grayscale');
  getFace($(this)).removeClass('grayscale');
}
$rateInputs3.on('change', handleRateInputChange3);

/*Function RateInput4*/
function handleRateInputChange4() {
  getFace($rateInputs4).addClass('grayscale');
  getFace($(this)).removeClass('grayscale');
}
$rateInputs4.on('change', handleRateInputChange4);


