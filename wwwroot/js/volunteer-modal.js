document.addEventListener("DOMContentLoaded", function () {
    const modalEl = document.getElementById("volunteerModal");
    const modal = new bootstrap.Modal(modalEl, {
        backdrop: true,
        keyboard: true,
        focus: true
    });

    
    const toastEl = document.getElementById("toastNotification");
    const toast = new bootstrap.Toast(toastEl);
    const toastMessage = document.getElementById("toastMessage");

    //  Removing lingering backdrops &  Restore body scroll
    modalEl.addEventListener('hidden.bs.modal', function () {
        // Remove any lingering backdrops
        const backdrops = document.querySelectorAll('.modal-backdrop');
        backdrops.forEach(backdrop => backdrop.remove());
        
        document.body.classList.remove('modal-open');
        document.body.style.removeProperty('overflow');
        document.body.style.removeProperty('padding-right');
    });

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
                console.log('Volunteer data:', volunteer);

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
                        <p><strong>Available From:</strong> ${volunteer.availableFromDate ? new Date(volunteer.availableFromDate).toLocaleDateString() : "—"}</p>
                        <p><strong>Available Until:</strong> ${volunteer.availableUntilDate ? new Date(volunteer.availableUntilDate).toLocaleDateString() : "—"}</p>
                    </div>
                `;
                document.getElementById("volunteerDetailsContent").innerHTML = html;

                document.getElementById("approveBtn").onclick = () => updateStatus(id, "Approved");
                document.getElementById("rejectBtn").onclick = () => updateStatus(id, "Rejected");

                modal.show();
            } catch (err) {
                console.error(err);
                toastMessage.textContent = "Failed to load volunteer details.";
                toast.show();
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
                toastMessage.textContent = `Volunteer ${status.toLowerCase()} successfully!`;
                toast.show();

                // Update the badge in the table instantly
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
            toastMessage.textContent = `Failed to update status: ${err.message}`;
            toast.show();
        }
    }
});