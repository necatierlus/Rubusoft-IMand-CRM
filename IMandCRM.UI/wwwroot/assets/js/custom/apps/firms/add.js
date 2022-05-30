"use strict";
var KTCreateFirm = function () {
    var validator;
    var handleForm = function () {

        validator = FormValidation.formValidation(
            o,
            {
                fields: {
                    'FirstName': {
                        validators: {
                            notEmpty: {
                                message: 'Ad alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'LastName': {
                        validators: {
                            notEmpty: {
                                message: 'Soyad alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'EMail': {
                        validators: {
                            notEmpty: {
                                message: 'E-Mail alanının doldurulması zorunludur.'
                            },
                            emailAddress: {
                                message: "Lütfen geçerli bir E-Mail adresi giriniz."
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

        r.addEventListener('click', function (e) {
            e.preventDefault();
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
                                o.submit();
                            }
                        });


                    } else {
                        Swal.fire({
                            text: "Lütfen hatalı/eksik alanları düzeltiniz.",
                            icon: "error",
                            buttonsStyling: !1,
                            confirmButtonText: "Tamam, devam et!",
                            customClass: {
                                confirmButton: "btn btn-light"
                            }
                        }).then((function () {
                            KTUtil.scrollTop()
                        }))
                    }
                });
            }
        });

    };

    var e, t, i, o, r, s, a = [];
    return {
        init: function () {

            (t = document.querySelector("#kt_modal_create_firm")) && (e = new bootstrap.Modal(t)), i = document.querySelector("#kt_create_firm_stepper"), o = i.querySelector("#kt_create_firm_form"), r = i.querySelector('[data-kt-stepper-action="submit"]'), (s = new KTStepper(i)).on("kt.stepper.next", (function (e) {

                var t = a[e.getCurrentStepIndex() - 1];
                t ? t.validate().then((function (t) {
                    console.log("validated!"), "Valid" == t ? (e.goNext(), KTUtil.scrollTop()) : Swal.fire({
                        text: "Lütfen hatalı/eksik alanları düzeltiniz.",
                        icon: "error",
                        buttonsStyling: !1,
                        confirmButtonText: "Tamam, devam et!",
                        customClass: {
                            confirmButton: "btn btn-light"
                        }
                    }).then((function () {
                        KTUtil.scrollTop()
                    }))
                })) : (e.goNext(), KTUtil.scrollTop())
            })), s.on("kt.stepper.previous", (function (e) {
                console.log("stepper.previous"), e.goPrevious(), KTUtil.scrollTop()
            })), a.push(FormValidation.formValidation(o, {
                fields: {
                    FirmName: {
                        validators: {
                            notEmpty: {
                                message: "Firma adı alanı boş geçilemez."
                            }
                        }
                    },
                    FirmAddress: {
                        validators: {
                            notEmpty: {
                                message: "Firma adres alanı boş geçilemez."
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger,
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: ".fv-row",
                        eleInvalidClass: "",
                        eleValidClass: ""
                    })
                }
            })), a.push(FormValidation.formValidation(o, {
                fields: {
                    AddressTitle: {
                        validators: {
                            notEmpty: {
                                message: "Adres başlık alanı boş geçilemez."
                            }
                        }
                    },
                    CountryId: {
                        validators: {
                            notEmpty: {
                                message: "Ülke alanı boş geçilemez."
                            }
                        }
                    },
                    CityId: {
                        validators: {
                            notEmpty: {
                                message: "İl alanı boş geçilemez."
                            }
                        }
                    },
                    DistrictId: {
                        validators: {
                            notEmpty: {
                                message: "İlçe alanı boş geçilemez."
                            }
                        }
                    },
                    AddressDescription: {
                        validators: {
                            notEmpty: {
                                message: "Adres açıklama alanı boş geçilemez."
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger,
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: ".fv-row",
                        eleInvalidClass: "",
                        eleValidClass: ""
                    })
                }
            })),
                handleForm();


        }
    }
}();
KTUtil.onDOMContentLoaded((function () {
    KTCreateFirm.init()
}));


