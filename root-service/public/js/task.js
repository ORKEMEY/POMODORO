function onOpen() {
    const input = document.querySelector("input[type='text']");
    const ul = document.querySelector("ul.todos");
    const saveButton = document.getElementById("btn_create_new");
    const clearButton = document.querySelector("button.clear");
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
            localStorage.removeItem(deletekey);
            parent.remove()
        });



        ul.appendChild(li).append(textSpan, textSpan2, textSpan3, deleteBtn);
        input.value = "";

    };


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

    });



}

document.addEventListener("DOMContentLoaded", onOpen);
