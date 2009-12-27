function infostart_rate(ob_id, vote)
{
	$.get("/bitrix/components/infostart/objects_new.list/ajax_rate.php",
				{ID:ob_id, TYPE:vote},
				function(answer)
				{
					alert(answer);
				});
}