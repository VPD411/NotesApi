const api = '/api/notes';

const notesList = document.getElementById('notes-list');

async function loadNotes() {
    try {
        const response = await fetch(api);
        if (!response.ok) throw new Error(`HTTP ERROR: ${response.status}`);
        const notes = await response.json();
        renderNotes(notes);
    }
    catch (err) {
        console.log(`ERROR: ${err}`);
    }
}

function renderNotes(notes) {
    notesList.innerHTML = notes.map(n =>
        `
        <div class="note">
            <h3>${n.title}</h3>
            <p>${n.content}</p>
            <small>${new Date(n.createdAt).toLocaleString()}</small>
            <br>
            <button class="edit" onClick="editNote(${n.id}, '${n.title}', '${n.content}')">Изменить</button>
            <button class="delete" onClick="deleteNote(${n.id})">Удалить</button>
        </div>
        `).join('');
}

async function saveNote() {
    const id = document.getElementById('title').dataset.editId;
    const note = {
        title: document.getElementById('title').value,
        content: document.getElementById('content').value
    };

    const method = id ? 'PUT' : 'POST'
    const url = id ? `${api}/${id}` : api;

    await fetch(url, {
        method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(note)
    });

    resetForm();
    loadNotes();
}

function resetForm() {
    document.getElementById('title').value = '';
    document.getElementById('content').value = '';
    delete document.getElementById('title').dataset.editId;
}

function editNote(id, title, content) {
    document.getElementById('title').value = title;
    document.getElementById('content').value = content;
    document.getElementById('title').dataset.editId = id;
}

async function deleteNote(id) {
    await fetch(`${api}/${id}`, {
        method: 'DELETE'
    });

    loadNotes();
}

loadNotes();