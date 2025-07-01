// const API_HOST = 'https://localhost:7107';
// const API_HOST = 'https://elsayedelbassuoiny.runasp.net';
const API_HOST = 'https://studentreservation.runasp.net';

document.addEventListener('DOMContentLoaded', async () => {
    const groupFilter = document.getElementById('groupFilter');
    const reservationsBody = document.getElementById('reservationsBody');
    const exportButton = document.getElementById('exportButton');

    async function fetchGroups() {
        try {
            const response = await fetch(`${API_HOST}/group-status`);
            return await response.json();
        } catch (error) {
            console.error('Error fetching groups:', error);
            return [];
        }
    }

    async function populateGroupFilter() {
        const groups = await fetchGroups();
        groupFilter.innerHTML = '<option value="">كل المجموعات</option>';
        groups.forEach(group => {
            const option = document.createElement('option');
            option.value = group.id;
            option.textContent = group.name;
            groupFilter.appendChild(option);
        });
    }

    async function fetchReservations(groupId) {
        try {
            const url = groupId
                ? `${API_HOST}/api/Reservation/resingroup/${groupId}`
                : `${API_HOST}/api/Reservation/resingroup`;
            const response = await fetch(url);
         //   console.log(response.json());
            return await response.json();
        } catch (error) {
            console.error('Error fetching reservations:', error);
            return [];
        }
    }

    function displayReservations(reservations) {
        reservationsBody.innerHTML = '';
        reservations.forEach(reservation => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${reservation.name || '-'}</td>
                <td>${reservation.phone || '-'}</td>
                <td>${reservation.phoneParent1 || '-'}</td>
                <td>${reservation.phoneParent2 || '-'}</td>
                <td>${reservation.address || '-'}</td>
                <td>${reservation.email || '-'}</td>
                <td>${reservation.group || '-'}</td>
            `;
            reservationsBody.appendChild(row);
        });

        if (reservations.length === 0) {
            const emptyRow = document.createElement('tr');
            emptyRow.innerHTML = '<td colspan="7" style="text-align: center">لا توجد حجوزات</td>';
            reservationsBody.appendChild(emptyRow);
        }

        document.getElementById('rowCount').textContent = reservations.length;
    }

    groupFilter.addEventListener('change', async () => {
        const selectedGroup = groupFilter.value;
        const reservations = await fetchReservations(selectedGroup);
        displayReservations(reservations);
        console.log(reservations)
    });

    exportButton.addEventListener('click', async () => {
        try {
            exportButton.disabled = true;
            exportButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> جاري التصدير...';

            const reservations = await fetchReservations(groupFilter.value);

            const excelData = reservations.map(res => ({
                'الاسم': res.name || '-',
                'الهاتف': res.phone || '-',
                'هاتف ولي الأمر 1': res.phoneParent1 || '-',
                'هاتف ولي الأمر 2': res.phoneParent2 || '-',
                'العنوان': res.address || '-',
                'البريد الإلكتروني': res.email || '-',
                'المجموعة': res.group || '-'
            }));

            const worksheet = XLSX.utils.json_to_sheet(excelData);
            const workbook = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(workbook, worksheet, 'الحجوزات');

            XLSX.writeFile(workbook, `حجوزات_${new Date().toLocaleDateString()}.xlsx`);
        } catch (error) {
            console.error('Export error:', error);
            alert('حدث خطأ أثناء التصدير');
        } finally {
            exportButton.disabled = false;
            exportButton.textContent = 'تصدير';
        }
    });

    await populateGroupFilter();
    const initialReservations = await fetchReservations(null);
    displayReservations(initialReservations);
});
