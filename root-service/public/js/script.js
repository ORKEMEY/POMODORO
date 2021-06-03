var emailKey = "email";
var loginKey = "login";

async function register_async() {
    const response = await fetch("/api/authorization?login=" + document.getElementById("int_popup_3_login").value +
        "&password=" + CryptoJS.MD5(document.getElementById("int_popup_3_password").value).toString() +
        "&email=" + document.getElementById("int_popup_3_email").value, {
        method: "POST",
        headers: { "Accept": "application/json" },
    });
    let data = await response.json();

    if (response.ok === true) {

        sessionStorage.setItem(emailKey, data.email);
        sessionStorage.setItem(loginKey, data.login);
        output_greeting(data.login);
        replace_log_in_btn();
        document.location.href = "#";
    }
    else {
        var alert = document.getElementById("popup_3_alert");
        alert.textContent = data.errorText;
        alert.hidden = false;
        console.log("Error: ", response.status, data.errorText);
    }
}

function validate_register_form() {

    var alert = document.getElementById("popup_3_alert");
    alert.textContent = '';
    alert.hidden = true;
    var login = document.getElementById('int_popup_3_login').value;
    var password = document.getElementById('int_popup_3_password').value;
    var email = document.getElementById('int_popup_3_email').value;
    var trems = document.getElementById('int_popup_3_terms');
    
    if (login == '') {
        alert.textContent = "Name cann't be empty!";
        alert.hidden = false;
        return false;
    }
    if (password == '') {
        alert.textContent += "Password cann't be empty!";
        alert.hidden = false;
        return false;
    }
    if (email == '') {
        alert.textContent += "E-Mail cann't be empty!";
        alert.hidden = false;
        return false;
    }
    if (trems.checked === false) {
        alert.textContent += "Accept terms and cons";
        alert.hidden = false;
        return false;
    }
    return true;
}

document.getElementById('btn_sign_up').onclick = function () {
    document.getElementById("popup_3_alert").hidden = true;
    if (validate_register_form()){ 
        register_async();
    }
};
//-------------------authorization---------------------------

async function log_in_async() {

    const response = await fetch("/api/authorization?email=" + document.getElementById("int_popup_2_email").value +
        "&password=" + CryptoJS.MD5(document.getElementById("int_popup_2_password").value).toString(), {
        method: "GET",
        headers: { "Accept": "application/json" },
    });
    let data = await response.json();
    
    // если запрос прошел нормально
    if (response.ok === true) {

        sessionStorage.setItem(emailKey, data.email);
        sessionStorage.setItem(loginKey, data.login);
        
        output_greeting(data.login);
        replace_log_in_btn();
        document.location.href = "#";
    }
    else {
        var alert = document.getElementById("popup_2_alert");
        alert.textContent = data.errorText;
        alert.hidden = false;
        console.log("Error: ", response.status, data.errorText);
    }
};

function validate_log_in_form() {

    var alert = document.getElementById("popup_2_alert");
    alert.textContent = '';
    alert.hidden = true;
    var email = document.getElementById('int_popup_2_email').value;
    var password = document.getElementById('int_popup_2_password').value;
    if (email == '') {
        alert.textContent += "E-Mail cann't be empty!";
        alert.hidden = false;
        return false;
    }
    if (password == '') {
        alert.textContent += "Password cann't be empty!";
        alert.hidden = false;
        return false;
    }
    return true;
}

document.getElementById('btn_sign_in').onclick = function () {
    document.getElementById("popup_2_alert").hidden = true;
    if (validate_log_in_form()) {
        log_in_async();
    }
};
//-----------------menu---------------------

function output_greeting(login){
    let p = document.createElement('p');
    p.id = "p_greeting";
    p.className = "link";
    p.textContent = "Hi, " + login.toString();

    let li = document.createElement('li');
    li.appendChild(p);

    document.getElementById("a_log_in").parentNode.parentNode.appendChild(li);
    document.getElementById("name").innerText = login.toString();
} 

function hide_greeting(){
    document.getElementById("p_greeting").replaceWith();
} 

function replace_log_in_btn(){
    let p = document.createElement('p');
    p.id = "a_log_out";
    p.className = "link";
    p.textContent = "Log out";
    p.onclick = function () {
        sessionStorage.removeItem(emailKey);
        sessionStorage.removeItem(loginKey);
        replace_log_out_btn();
        hide_greeting();
    };
    document.getElementById("a_log_in").replaceWith(p);
}

function replace_log_out_btn(){

    let a = document.createElement('a');
    a.id = "a_log_in";
    a.className = "link";
    a.textContent = "Log in";
    a.href = "#popup_2";
    document.getElementById("a_log_out").replaceWith(a);
}
