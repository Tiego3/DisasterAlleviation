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

    if (notificationType) {
        const type = notificationType.getAttribute("content");
        const message = notificationMessage ? notificationMessage.getAttribute("content") : "";

        showToastNotification(type, message);
    }

     function showToastNotification(type, message) {
        const toastEl = document.getElementById("notificationToast");
        const toastTitle = document.getElementById("toastTitle");
        const toastBody = document.getElementById("toastBody");

        if (!toastEl || !toastTitle || !toastBody) return;

        // Remove old classes and add new styling
        toastEl.className = "toast align-items-center border-0";

        // Configure toast based on type
        if (type === "donate-anonymous") {
            // Anonymous Monetary Donation
            toastEl.classList.add("text-bg-success");
            toastTitle.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Thank You for Your Donation!';
            toastBody.innerHTML = `
            <div class="mb-2">Your <strong>Anonymous Donor ID</strong> is:</div>
            <div class="bg-white bg-opacity-25 p-2 rounded text-center mb-2">
                <h5 class="fw-bold mb-0">${message}</h5>
            </div>
            <small class="opacity-75">Please save this ID for future donations.</small>
        `;

            const toast = new bootstrap.Toast(toastEl, { autohide: false });
            toast.show();

            toastEl.addEventListener('hidden.bs.toast', function () {
                window.location.href = "/Index";
            }, { once: true });

        } else if (type === "donate-named") {
            // Named Monetary Donation
            toastEl.classList.add("text-bg-success");
            toastTitle.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Thank You for Your Donation!';
            toastBody.innerHTML = `
            <div class="mb-2">Thank you, <strong>${message}</strong>!</div>
            <div class="opacity-90">Your generous contribution helps us provide critical relief to communities in crisis.</div>
            <div class="mt-3 opacity-75"><small>Your donation has been recorded successfully.</small></div>
        `;

            const toast = new bootstrap.Toast(toastEl, { autohide: true, delay: 4000 });
            toast.show();

            toastEl.addEventListener('hidden.bs.toast', function () {
                window.location.href = "/Index";
            }, { once: true });

        } else if (type === "volunteer") {
            // Volunteer Application
            toastEl.classList.add("text-bg-success");
            toastTitle.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Application Submitted!';
            toastBody.innerHTML = `<div class="opacity-90">${message}</div>`;

            const toast = new bootstrap.Toast(toastEl, { autohide: true, delay: 3000 });
            toast.show();

            toastEl.addEventListener('hidden.bs.toast', function () {
                window.location.href = "/Index";
            }, { once: true });

        } else if (type === "goods-donation") {
            // Goods Donation
            const referenceNumber = document.querySelector('meta[name="notification-reference"]')?.getAttribute('content');
            const dropoffMethod = document.querySelector('meta[name="notification-method"]')?.getAttribute('content');
            const dropoffDateTime = document.querySelector('meta[name="notification-datetime"]')?.getAttribute('content');
            const isAnonymous = document.querySelector('meta[name="notification-anonymous"]')?.getAttribute('content') === 'true';
            const anonId = document.querySelector('meta[name="notification-anonid"]')?.getAttribute('content');
            const donorName = document.querySelector('meta[name="notification-donorname"]')?.getAttribute('content');

            toastEl.classList.add("text-bg-success");
            toastTitle.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Goods Donation Submitted!';

            let bodyContent = '';

            // Thank you message based on donor type
            if (isAnonymous) {
                bodyContent += `<div class="mb-3">Thank you for your generous donation!</div>`;
            } else if (donorName) {
                bodyContent += `<div class="mb-3">Thank you, <strong>${donorName}</strong>, for your generous donation!</div>`;
            }

            // Reference number (always show for goods donations)
            if (referenceNumber) {
                bodyContent += `
                <div class="mb-2">Your <strong>Reference Number</strong> is:</div>
                <div class="bg-white bg-opacity-25 p-2 rounded text-center mb-3">
                    <h5 class="fw-bold mb-0">${referenceNumber}</h5>
                </div>
            `;
            }

            // Anonymous ID (only for anonymous donors)
            if (isAnonymous && anonId) {
                bodyContent += `
                <div class="mb-2">Your <strong>Anonymous Donor ID</strong> is:</div>
                <div class="bg-white bg-opacity-25 p-2 rounded text-center mb-3">
                    <h5 class="fw-bold mb-0">${anonId}</h5>
                </div>
            `;
            }

            // Drop-off information
            if (dropoffMethod === "Scheduled" && dropoffDateTime) {
                const formattedDate = new Date(dropoffDateTime).toLocaleString('en-US', {
                    year: 'numeric',
                    month: 'long',
                    day: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit'
                });
                bodyContent += `
                <div class="mt-2">
                    <small class="opacity-75">
                        <i class="bi bi-calendar-event me-1"></i>
                        Scheduled for: ${formattedDate}
                    </small>
                </div>
            `;
            } else if (dropoffMethod === "Immediate") {
                bodyContent += `
                <div class="mt-2">
                    <small class="opacity-75">
                        <i class="bi bi-clock me-1"></i>
                        Please present your reference number upon arrival.
                    </small>
                </div>
            `;
            }

            // Footer message
            bodyContent += `
            <div class="mt-2">
                <small class="opacity-75">
                    ${isAnonymous
                    ? 'Please save both your reference number and anonymous ID for your records.'
                    : 'Please save your reference number for your records.'}
                </small>
            </div>
        `;

            toastBody.innerHTML = bodyContent;

            // Show toast (manual dismiss - important info)
            const toast = new bootstrap.Toast(toastEl, { autohide: false });
            toast.show();

            toastEl.addEventListener('hidden.bs.toast', function () {
                window.location.href = "/Index";
            }, { once: true });
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