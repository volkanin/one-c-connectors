function infostart_rate(ob_id, vote)
{
	$.get("/bitrix/components/infostart/objects_new.list_detail/ajax_rate.php",
				{ID:ob_id, TYPE:vote},
				function(answer)
				{
					alert(answer);
				});
}

function infostart_rate_comment(comm_id, vote, ob_id)
{
	$.get("/bitrix/components/infostart/objects_new.list_detail/ajax_rate.php",
				{CID:comm_id, TYPE:vote, OBJ:ob_id},
				function(answer)
				{
					alert(answer);
				});
}

$(document).ready(function(){
	$('.comment-ref').cluetip({
							activation: 'click', 
							width: 350, 
							attribute: 'href',
							closeText: 'Закрыть', 
							closePosition: 'title',
							sticky: true
							});
});