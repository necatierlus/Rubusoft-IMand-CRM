// Class definition
var KTProductsEdit = function () {
    var submitButton;
    var validator;
    var form;


    // Init form inputs
    var handleForm = function () {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'ProductName': {
                        validators: {
                            notEmpty: {
                                message: 'Ürün Adı alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'ProductCode': {
                        validators: {
                            notEmpty: {
                                message: 'Ürün Kodu alanının doldurulması zorunludur.'
                            }
                        }
                    }

                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleInvalidClass: '',
                        eleValidClass: ''
                    })
                }
            }
        );

        // Action buttons
        submitButton.addEventListener('click', function (e) {
            e.preventDefault();
            debugger;
            if (validator) {
                validator.validate().then(function (status) {
                    if (status == 'Valid') {
                        Swal.fire({
                            title: "Kaydetmek istediğinizden emin misiniz?",
                            icon: "warning",
                            showCancelButton: true,
                            cancelButtonText: "Hayır!",
                            confirmButtonText: "Evet!"
                        }).then(function (result) {
                            if (result.value) {
                                form.submit();
                            }
                        });

                    } else {

                    }
                });
            }
        });

    }

    return {
        // Public functions
        init: function () {
            // Elements
            form = document.querySelector('#kt_products_edit_form');
            submitButton = form.querySelector('#kt_edit_product_submit');
            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTProductsEdit.init();
});