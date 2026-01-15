// ==========================================
// SITE.JS - Consolidated JavaScript
// ==========================================

document.addEventListener("DOMContentLoaded", function () {

    // ==========================================
    // DISASTER CARDS TOGGLE (Index page)
    // ==========================================
    const disasterGrid = document.getElementById("disasterGrid");
    const toggleBtn = document.getElementById("toggleDisastersBtn");

    if (disasterGrid && toggleBtn) {
        const cards = Array.from(disasterGrid.children);
        const maxVisible = 3;
        let expanded = false;

        // Initially show only top 3
        cards.forEach((card, index) => {
            if (index >= maxVisible) {
                card.style.display = "none";
            }
        });

        toggleBtn.addEventListener("click", () => {
            expanded = !expanded;
            cards.forEach((card, index) => {
                card.style.display = expanded || index < maxVisible ? "block" : "none";
            });
            toggleBtn.textContent = expanded ? "Show Less" : "View All Disasters";
        });
    }

    // ==========================================
    // NOTIFICATION HANDLING (Toast System)
    // ==========================================
    const notificationType = document.querySelector('meta[name="notification-type"]');
    const notificationMessage = document.querySelector('meta[name="notification-message"]');

    if (notificationType && notificationMessage) {
        const type = notificationType.getAttribute("content");
        const message = notificationMessage.getAttribute("content");

        showToastNotification(type, message);
    }

    function showToastNotification(type, message) {
        const toastEl = document.getElementById("notificationToast");
        const toastTitle = document.getElementById("toastTitle");
        const toastBody = document.getElementById("toastBody");

        if (!toastEl || !toastTitle || !toastBody) return;

        // Configure toast based on type
        if (type === "donate") {
            toastTitle.textContent = "Thank You for Your Donation!";
            toastBody.innerHTML = `
                <p class="mb-2">Your <strong>Anonymous Donor ID</strong> is:</p>
                <h5 class="text-primary fw-bold mb-2">${message}</h5>
                <p class="mb-0 small">Please save this ID for future donations.</p>
            `;

            // Show toast (manual dismiss only - no autohide)
            const toast = new bootstrap.Toast(toastEl, { autohide: false });
            toast.show();

            // Redirect only after user closes the toast
            toastEl.addEventListener('hidden.bs.toast', function () {
                window.location.href = "/Index";
            });

        } else if (type === "volunteer") {
            toastTitle.textContent = "Application Submitted!";
            toastBody.innerHTML = `<p class="mb-0">${message}</p>`;

            // Show toast with auto-hide after 3 seconds
            const toast = new bootstrap.Toast(toastEl, { autohide: true, delay: 3000 });
            toast.show();

            // Redirect after toast is hidden
            toastEl.addEventListener('hidden.bs.toast', function () {
                window.location.href = "/Index";
            });
        } else if (type === "goods-donation") {
            const reference = document.querySelector('meta[name="notification-reference"]')?.getAttribute('content');
            const method = document.querySelector('meta[name="notification-method"]')?.getAttribute('content');
            const datetime = document.querySelector('meta[name="notification-datetime"]')?.getAttribute('content');

            toastTitle.textContent = "Donation Submitted!";

            if (method === "Scheduled") {
                toastBody.innerHTML = `
            <p class="mb-2"><strong>Your goods donation has been scheduled.</strong></p>
            <div class="bg-light p-2 rounded mb-2">
                <p class="mb-1"><strong>Reference Number:</strong></p>
                <h5 class="text-primary fw-bold mb-0">${reference}</h5>
            </div>
            <p class="mb-1"><strong>Drop-off Time:</strong> ${datetime}</p>
            <p class="mb-0 small text-muted">Please present this reference number when you arrive.</p>
        `;
            } else {
                toastBody.innerHTML = `
            <p class="mb-2"><strong>Your donation is ready for drop-off.</strong></p>
            <div class="bg-light p-2 rounded mb-2">
                <p class="mb-1"><strong>Reference Number:</strong></p>
                <h5 class="text-primary fw-bold mb-0">${reference}</h5>
            </div>
            <p class="mb-0 small text-muted">Present this number at our facility.</p>
        `;
            }

            const toast = new bootstrap.Toast(toastEl, { autohide: false });
            toast.show();

            toastEl.addEventListener('hidden.bs.toast', function () {
                window.location.href = "/Index";
            });
        }
    }

    // ==========================================
    // DONATE FORM - ANONYMOUS TOGGLE
    // ==========================================
    const monetaryAnon = document.getElementById("monetaryAnonymous");
    const goodsAnon = document.getElementById("goodsAnonymous");

    if (monetaryAnon) {
        monetaryAnon.addEventListener("change", function () {
            toggleAnonymousFields(
                this,
                "monetaryNameGroup",
                "monetaryEmailGroup",
                "monetaryAnonIdGroup",
                "monetaryName",
                "monetaryEmail"
            );
        });
    }

    if (goodsAnon) {
        goodsAnon.addEventListener("change", function () {
            toggleAnonymousFields(
                this,
                "goodsNameGroup",
                "goodsEmailGroup",
                "goodsAnonIdGroup",
                "goodsName",
                "goodsEmail"
            );
        });
    }

    function toggleAnonymousFields(checkbox, nameGroupId, emailGroupId, anonIdGroupId, nameInputId, emailInputId) {
        const nameGroup = document.getElementById(nameGroupId);
        const emailGroup = document.getElementById(emailGroupId);
        const anonIdGroup = document.getElementById(anonIdGroupId);
        const nameInput = document.getElementById(nameInputId);
        const emailInput = document.getElementById(emailInputId);

        if (checkbox.checked) {
            nameGroup.style.display = "none";
            emailGroup.style.display = "none";
            anonIdGroup.style.display = "block";
            nameInput.value = "";
            emailInput.value = "";
        } else {
            nameGroup.style.display = "block";
            emailGroup.style.display = "block";
            anonIdGroup.style.display = "none";
        }
    }

    // ==========================================
    // VOLUNTEER MODAL (AdminDashboard)
    // ==========================================
    const modalEl = document.getElementById("volunteerModal");

    if (modalEl) {
        const modal = new bootstrap.Modal(modalEl);
        const toastEl = document.getElementById("toastNotification");
        const toast = toastEl ? new bootstrap.Toast(toastEl) : null;
        const toastMessage = document.getElementById("toastMessage");

        function getCsrfToken() {
            const meta = document.querySelector('meta[name="csrf-token"]');
            if (meta) return meta.getAttribute('content');
            const input = document.querySelector('input[name="__RequestVerificationToken"]');
            return input ? input.value : null;
        }

        document.querySelectorAll(".view-details-btn").forEach(btn => {
            btn.addEventListener("click", async function () {
                const id = this.getAttribute("data-id");
                try {
                    const response = await fetch(`?handler=VolunteerDetails&id=${id}`);
                    if (!response.ok) throw new Error(`Failed to load details: ${response.status}`);
                    const volunteer = await response.json();

                    const html = `
                        <div class="col-md-6">
                            <h6 class="fw-bold mb-2">Contact Information</h6>
                            <p class="mb-1"><i class="bi bi-envelope"></i> ${volunteer.email}</p>
                            <p class="mb-1"><i class="bi bi-telephone"></i> ${volunteer.phoneNumber}</p>
                            <p><i class="bi bi-geo-alt"></i> ${volunteer.location}</p>
                            <h6 class="fw-bold mt-3">Skills & Experience</h6>
                            <p>${volunteer.skills || "—"}</p>
                            <h6 class="fw-bold mt-3">Emergency Contact</h6>
                            <p>${volunteer.emergencyContact || "—"}</p>
                        </div>
                        <div class="col-md-6">
                            <h6 class="fw-bold mb-2">Availability</h6>
                            <p><strong>Schedule:</strong> ${volunteer.availability || "—"}</p>
                            <p><strong>Transportation:</strong> ${volunteer.hasTransportation ? "Yes" : "No"}</p>
                            <p><strong>Can Travel:</strong> ${volunteer.willingToTravel ? "Yes" : "No"}</p>
                            <p><strong>Date Available:</strong> ${volunteer.availableDates ? new Date(volunteer.availableDates).toLocaleDateString() : "—"}</p>
                        </div>
                    `;
                    document.getElementById("volunteerDetailsContent").innerHTML = html;

                    document.getElementById("approveBtn").onclick = () => updateStatus(id, "Approved");
                    document.getElementById("rejectBtn").onclick = () => updateStatus(id, "Rejected");

                    modal.show();
                } catch (err) {
                    console.error(err);
                    if (toastMessage && toast) {
                        toastMessage.textContent = "Failed to load volunteer details.";
                        toast.show();
                    }
                }
            });
        });

        async function updateStatus(id, status) {
            const confirmMsg = `Are you sure you want to mark this volunteer as ${status}?`;
            if (!confirm(confirmMsg)) return;

            const token = getCsrfToken();
            const url = `?handler=UpdateStatus&id=${encodeURIComponent(id)}&status=${encodeURIComponent(status)}`;

            try {
                const resp = await fetch(url, {
                    method: "POST",
                    headers: {
                        "RequestVerificationToken": token || "",
                        "Accept": "application/json"
                    }
                });

                if (!resp.ok) {
                    const txt = await resp.text();
                    throw new Error(`Server returned ${resp.status}: ${txt}`);
                }

                const result = await resp.json();
                if (result?.success) {
                    modal.hide();
                    if (toastMessage && toast) {
                        toastMessage.textContent = `Volunteer ${status.toLowerCase()} successfully!`;
                        toast.show();
                    }

                    // Update badge in table
                    const row = document.getElementById(`row-${id}`);
                    if (row) {
                        const badge = row.querySelector(".badge");
                        if (badge) {
                            badge.className = `badge bg-${status === "Approved" ? "success" : "danger"}`;
                            badge.textContent = status;
                        }
                    }
                } else {
                    throw new Error("Unexpected response from server.");
                }
            } catch (err) {
                console.error("updateStatus error:", err);
                if (toastMessage && toast) {
                    toastMessage.textContent = `Failed to update status: ${err.message}`;
                    toast.show();
                }
            }
        }
    }
});