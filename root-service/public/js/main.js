

var setting_minutes = 25;
var setting_short_break = 5;
var setting_long_break = 15;

var current_long_break = {
  seconds : 0,
  minutes : setting_long_break,
  reset : function (min = setting_long_break, sec = 0){
    pause_timer();
    this.seconds = sec;
    this.minutes = min;
    template(this);
   }
}

var current_short_break = {
  seconds : 0,
  minutes : setting_short_break,
  reset : function (min = setting_short_break, sec = 0){
    pause_timer();
    this.seconds = sec;
    this.minutes = min;
    template(this);
   } 
}

var current_main_session = {
   seconds : 0,
   minutes : setting_minutes,
   reset : function (min = setting_minutes, sec = 0){
    pause_timer();
    this.seconds = sec;
    this.minutes = min;
    template(this);
   } 
}

var current_session = current_main_session;

var timer_is_running = false

var timer_interval = null;

function template(session) {
  output_minutes(session);
  output_seconds(session);
}
function template() {
  output_minutes(current_session);
  output_seconds(current_session);
}

function pause_timer(){
        clearInterval(timer_interval);
        timer_is_running = false;
        document.getElementById("i_btn_play_pause").className = "far fa-caret-square-right";
}

function start_timer(session) {
  click_sound.play();
  if(timer_is_running) {
      pause_timer();
      return;
  }

  timer_is_running = true;
  document.getElementById("i_btn_play_pause").className = "fas fa-stop";

  output_minutes(session);
  output_seconds(session);
  
  timer_interval = setInterval(Timer, 1000);

  function minutesTimer() {
    session.minutes = session.minutes - 1;
    output_minutes(session);
  }

  function secondsTimer(){
    session.seconds = session.seconds - 1;
    output_seconds(session);
  }

  function Timer() {
    
    if (Number(session.seconds) <= 0) {
      if (session.minutes <= 0) {
        pause_timer();
        document.getElementById("done").innerHTML =
          "Session Completed!! Take a Break";
        document.getElementById("done").classList.add("show_message");
        bell.play();
        return;
      }
      minutesTimer();
      session.seconds = 60;
    }
    secondsTimer();
    
  }
}

function output_minutes(session){
  if(session.minutes < 10) document.getElementById("minutes").innerHTML = "0" + session.minutes;
    else document.getElementById("minutes").innerHTML = session.minutes;
}

function output_seconds(session){
  if(session.seconds < 10) document.getElementById("seconds").innerHTML = "0" + session.seconds;
    else document.getElementById("seconds").innerHTML = session.seconds;
}

document.getElementById('btn_play_pause').onclick = function () {
  start_timer(current_session);
};

document.getElementById('btn_reset').onclick = function () {
  function hide_comletion_message(){
    document.getElementById("done").innerHTML = '';
    document.getElementById("done").classList.remove("show_message");
  }
  hide_comletion_message();
  current_session.reset();
};

document.getElementById('btn_pomodoro').onclick = function () {
  pause_timer();
  current_session = current_main_session;
  template(current_session);
};

document.getElementById('btn_short_break').onclick = function () {
  pause_timer();
  current_session = current_short_break;
  template(current_session);
};

document.getElementById('btn_long_break').onclick = function () {
  pause_timer();
  current_session = current_long_break;
  template(current_session);
};

//-------settings-----------

function btn_popup_ok() {
  if(!validate_settings()) {
    return;
  }
  setting_minutes = document.getElementById("int_popup_set_minutes").value;
  setting_short_break = document.getElementById("int_popup_set_sh_br").value;
  setting_long_break = document.getElementById("int_popup_set_lg_br").value;

  current_main_session.reset();
  current_long_break.reset();
  current_short_break.reset();

  if(document.getElementById("int_popup_autostart").checked){
    start_timer(current_session);
  }
  document.location.href = "#";
};

function validate_settings() {
  var alert = document.getElementById("popup_alert");
  alert.textContent = '';
  alert.hidden = true;

  var minutes = document.getElementById('int_popup_set_minutes').value;
  var short_break = document.getElementById('int_popup_set_sh_br').value;
  var long_break = document.getElementById('int_popup_set_lg_br').value;

  if (minutes <=0 || minutes >= 100) {
      alert.textContent += "E-Mail cann't be empty!";
      alert.hidden = false;
      return false;
  }
  if (short_break <=0 || short_break >= 100) {
    alert.textContent += "E-Mail cann't be empty!";
    alert.hidden = false;
    return false;
  }
  if (long_break <=0 || long_break >= 100) {
    alert.textContent += "E-Mail cann't be empty!";
    alert.hidden = false;
    return false;
  }
  return true;
}