var input = document.querySelector("#phone");
var iti = window.intlTelInput(input, {
    // allowDropdown: false,
    //autoHideDialCode: false,
    //autoPlaceholder: "off",
    // dropdownContainer: document.body,
    // excludeCountries: ["us"],
    // formatOnDisplay: false,
    // geoIpLookup: function(callback) {
    //   $.get("http://ipinfo.io", function() {}, "jsonp").always(function(resp) {
    //     var countryCode = (resp && resp.country) ? resp.country : "";
    //     callback(countryCode);
    //   });
    // },
    hiddenInput: "FullPhone",
    // initialCountry: "auto",
    // localizedCountries: { 'de': 'Deutschland' },
    // nationalMode: false,
    // onlyCountries: ['us', 'gb', 'ch', 'ca', 'do'],
    // placeholderNumberType: "MOBILE",
    // preferredCountries: ['cn', 'jp'],
    //separateDialCode: true,
    utilsScript: "../Content/telephone-plugin/js/utils.js"
});


var errorMap = ["Invalid number", "Invalid country code", "Too short", "Too long", "Invalid number"];
//var input = document.querySelector("#phone"),
    errorMsg = document.querySelector("#error-msg");
validMsg = document.querySelector("#valid-msg");

var reset = function () {
    input.classList.remove("error");
    errorMsg.innerHTML = "";
    errorMsg.classList.add("hide");
    validMsg.classList.add("hide");
};

input.addEventListener('keyup', function () {
    reset();
    if (input.value.trim()) {
        if (iti.isValidNumber()) {
            validMsg.classList.remove("hide");
        } else {
            input.classList.add("error");
            var errorCode = iti.getValidationError();
            errorMsg.innerHTML = errorMap[errorCode];
            errorMsg.classList.remove("hide");
        }
    }
});

// on keyup / change flag: reset
//input.addEventListener('change', reset);
    //input.addEventListener('keyup', reset);