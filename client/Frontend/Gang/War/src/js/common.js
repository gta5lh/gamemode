$(document).ready(function ($) {
  $(document).ready(function ($) {
    let time = $('.clock').attr('time');
    
    $('.clock').countdown(time, function(event) {
        let seconds = event.strftime('%T') * 1000;
        let date;
        if(seconds < 90000000){
          date = new Date(seconds).toISOString().substr(14, 5);
        }else{
          date = new Date(seconds).toISOString().substr(11, 8);
        }
        
        $(this).html(date)
    });
});
});
