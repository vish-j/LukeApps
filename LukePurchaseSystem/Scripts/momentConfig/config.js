// arabic date rules
moment.updateLocale("en", {
    week: {
        dow: 6, // First day of week is Saturday
        doy: 12 // First week of year must contain 1 January (7 + 6 - 1)
    }
});