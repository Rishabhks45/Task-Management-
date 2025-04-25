document.addEventListener('DOMContentLoaded', function() {
    // Task filtering
    const filterButtons = document.querySelectorAll('.filter-btn');
    const taskRows = document.querySelectorAll('tr[data-status]');

    filterButtons.forEach(button => {
        button.addEventListener('click', () => {
            // Remove active class from all buttons
            filterButtons.forEach(btn => btn.classList.remove('active'));
            // Add active class to clicked button
            button.classList.add('active');

            const status = button.getAttribute('data-status');

            // Show/hide rows based on status
            taskRows.forEach(row => {
                if (status === 'all') {
                    row.classList.remove('hidden');
                } else {
                    const rowStatus = row.getAttribute('data-status');
                    if (rowStatus === status) {
                        row.classList.remove('hidden');
                    } else {
                        row.classList.add('hidden');
                    }
                }
            });
        });
    });

    // Search functionality
    const searchInput = document.querySelector('.search-input');
    if (searchInput) {
        searchInput.addEventListener('input', function(e) {
            const searchTerm = e.target.value.toLowerCase();
            
            taskRows.forEach(row => {
                const title = row.querySelector('td:first-child').textContent.toLowerCase();
                if (title.includes(searchTerm)) {
                    row.classList.remove('hidden');
                } else {
                    row.classList.add('hidden');
                }
            });
        });
    }
});