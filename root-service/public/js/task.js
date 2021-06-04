
function onOpen() {
    const input = document.querySelector("input[type='text']");
    const ul = document.querySelector("ul.todos");

    const saveButton = document.getElementById("btn_create_new");
    const clearButton = document.querySelector("button.clear");

function createTodo() {
    const li = document.createElement("li");
    const val = document.getElementById("create_text").value;
    const val2 = document.getElementById("date_task").value;
    const select = document.getElementById("select_fr");
    const val3 = select.options[select.selectedIndex].value;
    
    const textSpan = document.createElement("span");
    const textSpan2 = document.createElement("span");
    const textSpan3 = document.createElement("span");

    textSpan.append(val);
    textSpan2.append(val3);
    textSpan3.append(val2);
   
    
    const deleteBtn = document.createElement("span");
    deleteBtn.classList.add("todo-trash");
    const icon = document.createElement("i");
    icon.classList.add("fas", "fa-trash-alt");
    deleteBtn.appendChild(icon);
    deleteBtn.addEventListener("click", ()=>{ const parent = deleteBtn.parentElement;
    parent.remove()});



    ul.appendChild(li).append(textSpan, textSpan2, textSpan3, deleteBtn);
    input.value = "";
   
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

});



}

document.addEventListener("DOMContentLoaded", onOpen);