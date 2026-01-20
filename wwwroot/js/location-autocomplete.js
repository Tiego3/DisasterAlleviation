/**
 * Location Autocomplete Helper
 * Adds Google Places autocomplete to location input fields
 */

function initializeLocationAutocomplete(inputId, options = {}) {
    const input = document.getElementById(inputId);
    if (!input) {
        console.warn(`Location input with id "${inputId}" not found`);
        return null;
    }

    // Wait for Google Maps API to load
    if (typeof google === 'undefined' || typeof google.maps === 'undefined') {
        console.warn('Google Maps API not loaded yet. Retrying...');
        setTimeout(() => initializeLocationAutocomplete(inputId, options), 500);
        return null;
    }

    // Default options - restrict to South Africa
    const autocompleteOptions = {
        types: ['(cities)'],
        componentRestrictions: { country: 'za' }, // South Africa
        fields: ['address_components', 'geometry', 'name', 'formatted_address'],
        ...options
    };

    // Create autocomplete instance
    const autocomplete = new google.maps.places.Autocomplete(input, autocompleteOptions);

    // Handle place selection
    autocomplete.addListener('place_changed', function () {
        const place = autocomplete.getPlace();

        if (!place.geometry) {
            // User entered a name that was not suggested
            console.log("No details available for input: '" + place.name + "'");
            return;
        }

        // Extract city and province
        let city = '';
        let province = '';

        if (place.address_components) {
            for (const component of place.address_components) {
                const types = component.types;
                if (types.includes('locality')) {
                    city = component.long_name;
                } else if (types.includes('administrative_area_level_1')) {
                    province = component.long_name;
                }
            }
        }

        // Format the location nicely
        const formattedLocation = city && province
            ? `${city}, ${province}`
            : place.formatted_address || place.name;

        // Update the input value
        input.value = formattedLocation;

        // Trigger change event for validation
        input.dispatchEvent(new Event('change', { bubbles: true }));
    });

    return autocomplete;
}

// Auto-initialize on page load
document.addEventListener('DOMContentLoaded', function () {
    // Wait a bit for Google API to load
    setTimeout(() => {
        // Initialize for common location fields
        const locationInputs = [
            'Location',           // Volunteer.cshtml
            'location',           // AdminRegisterDisaster.cshtml
            'Input_Location',     // Various admin forms
        ];

        locationInputs.forEach(id => {
            const input = document.getElementById(id);
            if (input && input.type === 'text') {
                initializeLocationAutocomplete(id);
            }
        });
    }, 1000);
});