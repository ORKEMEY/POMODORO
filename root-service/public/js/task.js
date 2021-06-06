var emailKey = "email";
var loginKey = "login";

const input = document.querySelector("input[type='text']");
const ul = document.querySelector("ul.todos");
const saveButton = document.getElementById("btn_create_new");
const clearButton = document.querySelector("button.clear");

function show_tasks(){

    
    for (let i = 0; i < localStorage.length; i++) {
        const key = localStorage.key(i);
        const stored = localStorage.getItem(key);
        const parseStored = JSON.parse(stored);
        const li = document.createElement("li");
        const textSpan = document.createElement("span");
        const textSpan2 = document.createElement("span");
        const textSpan3 = document.createElement("span");
        textSpan.append(parseStored.text);
        textSpan2.append(parseStored.date);
        textSpan3.append(parseStored.frequency);
        const deleteBtn = document.createElement("span");
        deleteBtn.classList.add("todo-trash");
        const icon = document.createElement("i");
        icon.classList.add("fas", "fa-trash-alt");
        icon.id = key;
        deleteBtn.appendChild(icon);
        deleteBtn.addEventListener("click", () => {
            const parent = deleteBtn.parentElement;
            let deletekey = icon.id;
            delete_task_async(localStorage.getItem(deletekey));
            localStorage.removeItem(deletekey);
            parent.remove()
        });

        ul.appendChild(li).append(textSpan, textSpan2, textSpan3, deleteBtn);
        input.value = "";

    };
}


function onOpen() {
    get_task_async();


    function createTodo() {
        const li = document.createElement("li");
        const val = document.getElementById("create_text").value;
        const val2 = document.getElementById("date_task").value;
        const select = document.getElementById("select_fr");
        const val3 = select.options[select.selectedIndex].value;
        const textSpan = document.createElement("span");
        const textSpan2 = document.createElement("span");
        const textSpan3 = document.createElement("span");
        if (val != 0 && val2 != 0 && val3 != 0) {
            const json = `{
          "text": "${val}",
          "date": "${val2}",
          "frequency": "${val3}"  
        }`;
            const value = JSON.parse(json);
            let newKey = localStorage.length;
            localStorage[`"${newKey}"`] = json;
            textSpan.append(value.text);
            textSpan2.append(value.date);
            textSpan3.append(value.frequency);
            const deleteBtn = document.createElement("span");
            deleteBtn.classList.add("todo-trash");
            const icon = document.createElement("i");
            icon.classList.add("fas", "fa-trash-alt");
            icon.id = newKey;
            deleteBtn.appendChild(icon);
            deleteBtn.addEventListener("click", () => {
                const parent = deleteBtn.parentElement;
                let deletekey1 = icon.id;
                localStorage.removeItem(deletekey1);
                parent.remove()
                delete_task_async(json);
            });


            ul.appendChild(li).append(textSpan, textSpan2, textSpan3, deleteBtn);
            input.value = "";

        } else alert("cann't be empty");

    }

    input.addEventListener("keypress", (keyPressed) => {
        const keyEnter = 13;
        if (keyPressed.which == keyEnter) {
            createTodo();
        }
    });


    function onClickTodo(event) {
        if (event.target.tagName === "LI") {
            event.target.classList.toggle("checked");
        }
    }

    ul.addEventListener("click", onClickTodo);

    saveButton.addEventListener("click", () => {
        createTodo();
        save_task_async(localStorage.getItem(`"${localStorage.length - 1}"`));
    });

}

document.addEventListener("DOMContentLoaded", onOpen);

async function delete_task_async(content) {

    var email = sessionStorage.getItem(emailKey);

    const response = await fetch("/api/task?email=" + email +
        "&content=" + content, {
        method: "DELETE",
        headers: { "Accept": "application/json" },
    });
    let data = await response.json();

    if (response.ok === true) {
        
        alert('task has been deleted');
    }
    else {
        console.log("Error: ", response.status, data.errorText);
    }
}

async function save_task_async(content) {

    var email = sessionStorage.getItem(emailKey);

    const response = await fetch("/api/task?email=" + email +
        "&content=" + content, {
        method: "POST",
        headers: { "Accept": "application/json" },
    });
    let data = await response.json();

    if (response.ok === true) {

        alert('task has been saved');
    }
    else {
        console.log("Error: ", response.status, data.errorText);
    }
}

async function get_task_async() {

    var email = sessionStorage.getItem(emailKey);

    const response = await fetch("/api/task?email=" + email, {
        method: "GET",
        headers: { "Accept": "application/json" },
    });
    let data = await response.json();

    if (response.ok === true) {

        data.forEach(element => {
            localStorage.setItem(`"${localStorage.length}"`, element.content);
        });
        show_tasks();
    }
    else {
        console.log("Error: ", response.status, data.errorText);
    }
}

document.getElementById("a_log_out").onclick = function(){
    localStorage.clear();
}