let comments = [];
loadComments();

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
        body : commentBody.value,
        time : Math.floor(Date.now() / 1000),
        ref: ref,
    }

    commentBody.value = '';

    comments.push(comment);

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