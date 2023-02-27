$(() => {
    $("#addCommentModal").on("show.bs.modal", evt => {
        let $related = $(evt.relatedTarget);
        let inReplyTo = $related.data('commentId');
        $('#addCommentText').val('');
        $('#addCommentInReplyTo').val(inReplyTo);
    })
});