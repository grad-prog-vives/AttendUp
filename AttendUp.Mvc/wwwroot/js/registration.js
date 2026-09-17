// Extra persons state: { uid, firstName, lastName, subActivityId, fromFamilyKey }
let extraPersons = [];
let uidCounter = 0;

function toggleFamily(familyId) {
    const body = document.getElementById('family-body-' + familyId);
    const arrow = document.getElementById('arrow-' + familyId);
    body.classList.toggle('open');
    arrow.textContent = body.classList.contains('open') ? '▲' : '▼';
}

function toggleMember(familyId, memberId, firstName, lastName) {
    const key = 'fam-' + familyId + '-' + memberId;
    const btn = document.getElementById('mbtn-' + familyId + '-' + memberId);
    const existing = extraPersons.findIndex(p => p.fromFamilyKey === key);

    if (existing >= 0) {
        extraPersons.splice(existing, 1);
        btn.classList.remove('added');
    } else {
        extraPersons.push({ uid: ++uidCounter, firstName, lastName, subActivityId: null, fromFamilyKey: key });
        btn.classList.add('added');
    }
    render();
}

function addManualPerson() {
    extraPersons.push({ uid: ++uidCounter, firstName: '', lastName: '', subActivityId: null, fromFamilyKey: null });
    render();
}

function removePerson(uid) {
    const p = extraPersons.find(p => p.uid === uid);
    if (p?.fromFamilyKey) {
        const parts = p.fromFamilyKey.split('-');
        const mbtn = document.getElementById('mbtn-' + parts[1] + '-' + parts[2]);
        if (mbtn) mbtn.classList.remove('added');
    }
    extraPersons = extraPersons.filter(p => p.uid !== uid);
    render();
}

function updateField(uid, field, value) {
    const p = extraPersons.find(p => p.uid === uid);
    if (p) p[field] = field === 'subActivityId' ? parseInt(value) : value;
}

function render() {
    const list = document.getElementById('added-persons-list');
    const inputs = document.getElementById('extra-persons-inputs');

    list.innerHTML = extraPersons.map((p, i) => `
        <div class="reg-added-person">
            <div class="reg-person-header">
                <span>Persoon ${i + 2}</span>
                <button type="button" class="reg-btn-remove" onclick="removePerson(${p.uid})" title="Verwijderen">&#x2715;</button>
            </div>
            <div class="field">
                <label>Voornaam</label>
                <input class="input no-icon" placeholder="Voornaam" value="${esc(p.firstName)}"
                       oninput="updateField(${p.uid},'firstName',this.value)" required />
            </div>
            <div class="field">
                <label>Naam</label>
                <input class="input no-icon" placeholder="Achternaam" value="${esc(p.lastName)}"
                       oninput="updateField(${p.uid},'lastName',this.value)" required />
            </div>
            <div class="field">
                <label>Activiteit</label>
                <select class="select" onchange="updateField(${p.uid},'subActivityId',this.value)" required>
                    <option value="" disabled ${p.subActivityId === null ? 'selected' : ''}>Kies activiteit</option>
                    ${subActivities.map(s => `<option value="${s.id}" ${s.id === p.subActivityId ? 'selected' : ''}>${esc(s.name)}</option>`).join('')}
                </select>
            </div>
        </div>
    `).join('');

    inputs.innerHTML = extraPersons.map((p, i) => `
        <input type="hidden" name="ExtraPersons[${i}].FirstName" value="${esc(p.firstName)}" />
        <input type="hidden" name="ExtraPersons[${i}].LastName" value="${esc(p.lastName)}" />
        <input type="hidden" name="ExtraPersons[${i}].SubActivityID" value="${p.subActivityId}" />
    `).join('');
}

function esc(str) {
    return String(str ?? '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
}

document.getElementById('reg-form').addEventListener('submit', render);
