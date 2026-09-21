var $lastClickedBtn = null;

function OnModalBegin() {
    var submitBtn = $('#Modal').find('button[type="submit"]');
    submitBtn.find('.indicator-label').hide();
    submitBtn.find('.indicator-progress').show();
    submitBtn.prop('disabled', true);
}

function OnModalSuccess(response) {
    var myModalEl = document.getElementById('Modal');
    var modal = bootstrap.Modal.getInstance(myModalEl);

    if (typeof response === "string" && response.trim().startsWith("<tr")) {
        modal.hide();

        toastr.success("Operation completed successfully!", "Success", {
            closeButton: true,
            progressBar: true,
            positionClass: "toast-top-right"
        });

        var table = $('#kt_doctors_table').DataTable();
        var $newRow = $(response);

        var isEdit = $lastClickedBtn && $lastClickedBtn.data('id') != null;

        if (isEdit) {
            var doctorId = $lastClickedBtn.data('id').toString();

            table.rows(function (idx, data, node) {
                return $(node).find('[data-id="' + doctorId + '"]').length > 0;
            }).remove();
        }

        table.row.add($newRow).draw(false);

        reinitDropdowns();

        $lastClickedBtn = null;

    } else {
        $('#Modal .modal-body').html(response);
    }
}

function showErrorMessage(xhr) {
    toastr.error("Failed to process your request. Please try again.");
}

function OnModalComplete() {
    var submitBtn = $('#Modal').find('button[type="submit"]');
    submitBtn.find('.indicator-label').show();
    submitBtn.find('.indicator-progress').hide();
    submitBtn.prop('disabled', false);
}

function reinitDropdowns() {
    var dropdownElementList = [].slice.call(document.querySelectorAll('[data-bs-toggle="dropdown"]'));
    dropdownElementList.map(function (dropdownToggleEl) {
        return new bootstrap.Dropdown(dropdownToggleEl);
    });
}

$(document).on('click', '#model-add-edit-js, [data-bs-target="#Modal"]', function (e) {
    e.preventDefault();
    $lastClickedBtn = $(this);

    var url = $lastClickedBtn.data('url') || $lastClickedBtn.attr('href');
    var title = $lastClickedBtn.data('title') || 'Add/Edit Doctor';

    $('#Modal .modal-title').text(title);
    $('#Modal .modal-body').html('<div class="text-center py-4"><div class="spinner-border text-primary" role="status"></div></div>');

    var modalElement = document.getElementById('Modal');
    var myModal = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
    myModal.show();

    if (url && url !== 'javascript:;') {
        $.get(url, function (data) {
            $('#Modal .modal-body').html(data);
        }).fail(function (xhr) {
            $('#Modal .modal-body').html('<div class="alert alert-danger text-center">Failed to load content. Status: ' + xhr.status + '</div>');
        });
    }
});

$(document).on('click', '.js-toggle-status', function (e) {
    e.preventDefault();
    var btn = $(this);
    var url = btn.data('url');
    var token = $('input[name="__RequestVerificationToken"]').val();

    bootbox.confirm({
        message: 'Are you sure you want to toggle status?',
        buttons: {
            confirm: { label: 'Yes', className: 'btn-success' },
            cancel: { label: 'No', className: 'btn-danger' }
        },
        callback: function (result) {
            if (!result) return;

            $.ajax({
                url: url,
                type: 'POST',
                data: { '__RequestVerificationToken': token },
                success: function (lastUpdatedOn) {
                    var row = btn.closest('tr');
                    var badgeSpan = row.find('.js-status').closest('.badge');

                    if (badgeSpan.hasClass('bg-light-success')) {
                        badgeSpan.removeClass('bg-light-success text-success').addClass('bg-light-danger text-danger');
                        badgeSpan.html('<i class="fa-solid fa-circle-xmark me-1 text-danger js-status"></i> Inactive');
                    } else {
                        badgeSpan.removeClass('bg-light-danger text-danger').addClass('bg-light-success text-success');
                        badgeSpan.html('<i class="fa-solid fa-circle-check me-1 text-success js-status"></i> Active');
                    }

                    row.find('td:nth-last-child(2)').text(lastUpdatedOn);
                    row.addClass('animate__animated animate__flash');
                    row.one('animationend webkitAnimationEnd oAnimationEnd', function () {
                        row.removeClass('animate__animated animate__flash');
                    });
                },
                error: function () {
                    Swal.fire({ icon: 'error', title: 'Oops...', text: 'Something went wrong!' });
                }
            });
        }
    });
});

$(document).on('click', '[data-bs-toggle="dropdown"]', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var $btn = $(this);
    var $parent = $btn.closest('.dropdown');

    $('.dropdown').not($parent).removeClass('show');
    $('.dropdown-menu').not($parent.find('.dropdown-menu')).removeClass('show');

    $parent.toggleClass('show');
    $parent.find('.dropdown-menu').toggleClass('show');
});

$(document).on('click', function (e) {
    if (!$(e.target).closest('.dropdown').length) {
        $('.dropdown').removeClass('show');
        $('.dropdown-menu').removeClass('show');
    }
});