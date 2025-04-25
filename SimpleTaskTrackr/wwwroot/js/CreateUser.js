
    document.addEventListener('DOMContentLoaded', function () {
        // Password show/hide toggle
        document.querySelectorAll('.toggle-password').forEach(icon => {
            icon.addEventListener('click', function () {
                const input = document.querySelector(this.getAttribute('toggle'));
                const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
                input.setAttribute('type', type);
                this.classList.toggle('fa-eye');
                this.classList.toggle('fa-eye-slash');
            });
        });

    // Form validation
    document.getElementById('signupForm').addEventListener('submit', function (e) {
        e.preventDefault();

    const name = document.getElementById('name').value.trim();
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    let isValid = true;

            // Reset errors
            document.querySelectorAll('.form-group').forEach(g => g.classList.remove('error'));

    if (name === '') {
        document.getElementById('name').closest('.form-group').classList.add('error');
    isValid = false;
            }

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        document.getElementById('email').closest('.form-group').classList.add('error');
    isValid = false;
            }

    const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=[\]{ };':"\\|,.<>/?]).{6,}$/;
        if (!passwordRegex.test(password)) {
            alert("Password must contain at least one uppercase letter, one number, and one special character.");
        document.getElementById('password').closest('.form-group').classList.add('error');
        isValid = false;
            }

        if (password !== confirmPassword) {
            document.getElementById('confirmPassword').closest('.form-group').classList.add('error');
        isValid = false;
            }

        if (isValid) {
            console.log('Form is valid:', { name, email, password });
        // You can submit form via AJAX or proceed
        this.submit();
            }
        });
    });

