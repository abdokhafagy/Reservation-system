// const API_BASE_URL = 'https://localhost:7107';
const API_BASE_URL = 'https://studentreservation.runasp.net';

document.addEventListener('DOMContentLoaded', async () => {
    // Check internet connection first
    if (!navigator.onLine) {
        alert('لا يوجد اتصال بالإنترنت. يرجى التحقق من اتصالك ثم إعادة تحميل الصفحة.');
        return;
    }

    const form = document.getElementById('registrationForm');
    const groupsList = document.getElementById('groupsList');
    groupsList.innerHTML = '<p>جاري تحميل المجموعات...</p>';

    // Add offline event listener
    window.addEventListener('offline', () => {
        alert('تم فقدان الاتصال بالإنترنت. يرجى إعادة الاتصال لمتابعة العمل.');
    });

    try {
        const resp = await fetch(`${API_BASE_URL}/group-status`);
        if (!resp.ok) throw new Error('فشل تحميل المجموعات');
        const groups = await resp.json();

        if (!groups.length) {
            groupsList.innerHTML = '<p>لا توجد مجموعات متاحة حالياً.</p>';
        } else {
            groupsList.innerHTML = ''; // Clear loading message

            for (const group of groups) {
                // Fetch actual count from Reservation API
                const countRes = await fetch(`${API_BASE_URL}/api/Reservation/Count/${group.id}`);
                const actualCount = await countRes.json();

                // Create elements
                const div = document.createElement('div');
                div.className = 'form-check';

                const input = document.createElement('input');
                input.type = 'radio';
                input.name = 'group';
                input.id = `group-${group.id}`;
                input.value = group.id;
                input.required = true;
                input.dataset.groupName = group.name;

                // Disable condition:
                if (
                    group.reservationCount === actualCount &&
                    actualCount !== 0
                ) {
                    input.disabled = true;
                }

                const label = document.createElement('label');
                label.className = 'form-check-label';
                label.htmlFor = input.id;

                const span = document.createElement('span');
                span.className = 'group-name';
                span.textContent = group.name;
                if (
                    group.reservationCount === actualCount &&
                    actualCount !== 0
                ) {
                    span.textContent += ' (مكتمل)';
                }

                label.appendChild(input);
                label.appendChild(span);
                div.appendChild(label);
                groupsList.appendChild(div);
            }
        }
    } catch (err) {
        groupsList.innerHTML = '<p class="text-danger">خطأ في تحميل المجموعات.</p>';
        console.error(err);
    }

    form.addEventListener('submit', async e => {
        e.preventDefault();
        const sel = document.querySelector('input[name="group"]:checked');
        if (!sel) {
            alert('يرجى اختيار مجموعة');
            return;
        }

        const formData = {
            name: document.getElementById('name').value.trim(),
            phone: document.getElementById('phone').value.trim(),
            phoneParent1: document.getElementById('phoneParent1').value.trim(),
            phoneParent2: document.getElementById('phoneParent2').value.trim(),
            address: document.getElementById('address').value.trim(),
            email: document.getElementById('email').value.trim(),
            group: sel.dataset.groupName,
            groupId: parseInt(sel.value)
        };

        try {
            const res = await fetch(`${API_BASE_URL}/api/Reservation`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData)
            });

            if (res.ok) {
                alert('تم التسجيل بنجاح!');
                form.reset();
                location.reload();
            } else if (res.status === 400) {
                const errorData = await res.json();
                alert(`طلب غير صالح: ${errorData.message || 'الرجاء التحقق من البيانات المدخلة'}`);
            } else {
                const err = await res.json();
                alert(err.message || 'فشل التسجيل');
            }
        } catch (error) {
            console.error(error);
            alert('حدث خطأ في الاتصال بالخادم');
        }
    });
});
