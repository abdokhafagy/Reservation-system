// Add this at the top of the file
// const API_HOST = 'https://localhost:7107';
const API_HOST = 'https://studentreservation.runasp.net';
// const API_HOST = 'https://elsayedelbassuoiny.runasp.net';

document.addEventListener('DOMContentLoaded', async () => {
    const groupForm = document.getElementById('groupForm');
    const groupsBody = document.getElementById('groupsBody');
    const resetFormButton = document.getElementById('resetForm');

    // Function to fetch all groups
    async function fetchGroups() {
        try {
            const response = await fetch(`${API_HOST}/group-status`);
            const data = await response.json();
            return data;
        } catch (error) {
            console.error('Error fetching groups:', error);
            return [];
        }
    }

    // Function to display groups in the table
    async function displayGroups() {
        const groups = await fetchGroups();
        groupsBody.innerHTML = '';
        groups.forEach(group => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>#${group.id}</td>
                <td>${group.name}</td>
                <td>${group.reservationCount}</td>
                <td>
                    <i class="fas fa-edit action-icon edit-icon" 
                       onclick="editGroup(${group.id}, '${group.name}', ${group.reservationCount})"
                       title="تعديل"></i>
                    <i class="fas fa-trash-alt action-icon delete-icon" 
                       onclick="deleteGroup(${group.id})"
                       title="حذف"
                       style="margin-right: 1rem;"></i>
                </td>
            `;
            groupsBody.appendChild(row);
        });
    }

    // Function to add/update group
    async function handleGroupSubmit(event) {
        event.preventDefault();
        const groupId = document.getElementById('groupId').value;
        const name = document.getElementById('groupName').value;
        const reservationCount = parseInt(document.getElementById('groupCount').value);

        const data = { name, reservationCount };
        const url = groupId
            ? `${API_HOST}/group/${groupId}`
            : `${API_HOST}/api/Groups`;

        try {
            const response = await fetch(url, {
                method: groupId ? 'PUT' : 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                await displayGroups();
                resetForm();
            } else {
                alert('حدث خطأ أثناء حفظ المجموعة');
            }
        } catch (error) {
            console.error('Error saving group:', error);
            alert('حدث خطأ في الاتصال');
        }
    }

    // Function to edit group
    window.editGroup = (id, name, count) => {
        document.getElementById('groupId').value = id;
        document.getElementById('groupName').value = name;
        document.getElementById('groupCount').value = count;
        document.querySelector('#groupForm button[type="submit"]').textContent = 'تحديث';
    };

    // Function to delete group
    window.deleteGroup = async (id) => {
        if (!confirm('هل أنت متأكد من حذف هذه المجموعة؟')) return;

        try {
            const response = await fetch(`${API_HOST}/group/${id}`, {
                method: 'DELETE'
            });

            if (response.ok) {
                await displayGroups();
            } else {
                alert('حدث خطأ أثناء حذف المجموعة');
            }
        } catch (error) {
            console.error('Error deleting group:', error);
            alert('حدث خطأ في الاتصال');
        }
    };

    // Function to reset form
    function resetForm() {
        document.getElementById('groupId').value = '';
        document.getElementById('groupName').value = '';
        document.getElementById('groupCount').value = '';
        document.querySelector('#groupForm button[type="submit"]').textContent = 'إضافة';
    }

    // Event listeners
    groupForm.addEventListener('submit', handleGroupSubmit);
    resetFormButton.addEventListener('click', resetForm);

    // Initialize
    await displayGroups();
});