// script.js
jQuery(function ($) {

    // 🔁 Toggle between login & registration
    const container = document.getElementById('auth-container');
    const switchBtn = document.getElementById('switch-btn');
    if (switchBtn) {
        switchBtn.addEventListener('click', () => container.classList.toggle('s--signup'));
    }

    // ✅ Email validation
    $("#Email").on("blur", function () {
        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,}$/;
        if (!emailPattern.test($(this).val())) {
            alert("❌ Please enter a valid email address!");
            $(this).val('');
        }
    });

    // ✅ Phone validation
    $("#Phone").on("input", function () {
        const phone = $(this).val();
        if (/[^\d]/.test(phone)) {
            alert("❌ Only digits allowed.");
            $(this).val(phone.replace(/[^\d]/g, ''));
            return;
        }
        if (!/^[6-9]\d*$/.test(phone)) {
            alert("❌ Phone number must start with 6, 7, 8, or 9.");
            $(this).val('');
        }
    });

    // ✅ Password validation
    $("#Password").on("input", function () {
        const val = $(this).val();
        const invalidChars = /[ \-_,]/;
        const validPattern = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/;

        if (invalidChars.test(val)) {
            alert("❌ Don't use space, hyphen, underscore, or comma.");
            $(this).val(val.replace(/[ \-_,]/g, ""));
        } else if (!validPattern.test(val)) {
            $("#registerMessage").text("⚠️ Password must have letters, digits, and one special character (@$!%*?&).");
        } else {
            $("#registerMessage").text("");
        }
    });
});
