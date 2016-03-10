$(function () {
    $.getJSON('/api/contact', function (contactsJsonPayload) {
        $(contactsJsonPayload).each(function (i, item) {
            $('#contacts').append('<li>' + item.Name + '</li>');
        });
    });
});

$('#saveContact').click(function () {
    var g = $.post("api/contact",
          $("#saveContactForm").serialize(),
          function (value) {
              Console.WriteLine(value);
              $('#contacts').append('<li>' + value.Name + '</li>');
          },
          "json"
    );
    g.fail(function() {
        alert("error");
    });
});