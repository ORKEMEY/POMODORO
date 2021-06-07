let comments = [];
get_message_async();
//loadComments();
var emailKey = "email";
var loginKey = "login";


document.getElementById('comment-add').onclick = function(){
    let commentBody = document.getElementById('comment-body');
    //находим ссылки на сообщение и если нет то null
    let ref = null;
    const refComment = document.querySelector('input[type=radio]:checked');
    if(refComment){
        const idComment = refComment.getAttribute("id");
        ref = idComment;
        refComment.checked = false;
    }
    //создаем id коммента, если комментов не было то id = 0
    let id = 0;
    const data = localStorage.getItem('comments');
    if(data) id = JSON.parse(data).length;

    let comment = {
        id: id,
        login : sessionStorage.getItem(loginKey),
        email : sessionStorage.getItem(emailKey),
        body : commentBody.value,
        time : Math.floor(Date.now() / 1000),
        ref: ref,
    }

    commentBody.value = '';
    comments.push(comment);
    
    if(comment.ref !== null){

        var replyComment = null;
        comments.forEach(element => {
            if(element.id == comment.ref){
                replyComment = element;
            }
        });
       // var replyComments = comments.map(x => x.id === comment.ref);

        if(replyComment !== null){
            reply_message_async(JSON.stringify(comment), replyComment.email, JSON.stringify(replyComment));
        }  
    }
    else save_message_async(JSON.stringify(comment));

    saveComments();
    showComments();

}

function clearRadio(){
    const clearButton = document.getElementById("clearbtn");
    if(!clearButton){
        const area = document.getElementById("btnarea")
        const newBtn = document.createElement("button");
        newBtn.innerText = "Clear"
        newBtn.setAttribute("id", "clearbtn");
        newBtn.addEventListener("click", () => {
            const refComment = document.querySelector('input[type=radio]:checked');
            if(refComment){
                refComment.checked = false;
            }
            const button = documnet.getElementById("clearbtn");
            button.remove();
        })
        area.append(newBtn)
    }
}

function saveComments(){
    localStorage.setItem('comments', JSON.stringify(comments));
}

function loadComments(){
    if (localStorage.getItem('comments')) comments = JSON.parse(localStorage.getItem('comments'));
    showComments();
}

function showComments (){
    let commentField = document.getElementById('comment-field');
    let out = '';

    comments.forEach(function(item){
        out += '<div>'
        out += `<input type="radio" id='${item.id}' onClick="clearRadio()" name="comment">`
        out += `<p class="text-right small"><em>${timeConverter(item.time)}</em></p>`;
        //если есть id ссылки добавляем доп поле с текстом коммента, на который ссылаемся
        if(item.ref !== null){
            out += `<p class="alert alert-success" role="alert">${comments[item.ref].body}</p>`

        }


        out += `<p class="alert alert-success" role="alert">${item.body}</p>`;
        out += '</div>'
    });
    
    commentField.innerHTML = out;
}

async function get_message_async() {

    const response = await fetch("/api/message", {
        method: "GET",
        headers: { "Accept": "application/json" },
    });
    let data = await response.json();

    if (response.ok === true) {

        data.forEach(element => {
            comments.push(JSON.parse(element.content));
        });
        saveComments();
        loadComments();
    }
    else {
        console.log("Error: ", response.status, data.errorText);
    }
}

async function reply_message_async(content, emailReply, contentReply) {

    var email = sessionStorage.getItem(emailKey);

    const response = await fetch("/api/message/reply?email=" + email +
        "&content=" + content + "&emailReply=" + emailReply
         + "&contentReply=" + contentReply, {
        method: "POST",
        headers: { "Accept": "application/json" },
    });
    let data = await response.json();

    if (response.ok === true) {

        alert('message has been saved');
    }
    else {
        console.log("Error: ", response.status, data.errorText);
    }
}

async function save_message_async(content) {

    var email = sessionStorage.getItem(emailKey);

    const response = await fetch("/api/message?email=" + email +
        "&content=" + content, {
        method: "POST",
        headers: { "Accept": "application/json" },
    });
    let data = await response.json();

    if (response.ok === true) {

        alert('message has been saved');
    }
    else {
        console.log("Error: ", response.status, data.errorText);
    }
}

function timeConverter(UNIX_timestamp){
    var a = new Date(UNIX_timestamp * 1000);
    var months = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    var year = a.getFullYear();
    var month = months[a.getMonth()];
    var date = a.getDate();
    var hour = a.getHours();
    var min = a.getMinutes();
    var sec = a.getSeconds();
    var time = date + ' ' + month + ' ' + year + ' ' + hour + ':' + min + ':' + sec ;
    return time;
  }

  document.getElementById("a_log_out").onclick = function(){
    localStorage.clear();
}