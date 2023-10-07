extends VBoxContainer
class_name ResourceUI

@onready var resource_name_label = %ResourceNameLabel
@onready var resource_amount_label = %ResourceAmountLabel
@onready var capacity_label = %CapacityLabel
@onready var delta_label = %DeltaLabel
@onready var amount_progress = %AmountProgress
@onready var game_controller = $/root/GameController

@export var resource_name = "wooood": 
    set(val):
        resource_name = val
        find_child("TooltipShower").tooltip_label_text = Query.ResourceDescription(resource_name)

func _process(delta):
    resource_name_label.text = Query.ResourceName(resource_name)
    var amount = Query.SettlementInventoryResourceAmount(game_controller.currentSettlementUuid, resource_name)
    var cap = Query.SettlementInventoryResourceCapacity(game_controller.currentSettlementUuid, resource_name)
    resource_amount_label.text = str(amount)
    capacity_label.text = "/"+str(cap)
    amount_progress.set_value_no_signal(amount / cap)
    var resource_delta = Query.SettlementInventoryResourceDelta(game_controller.currentSettlementUuid, resource_name)
    var delta_text = str(resource_delta) + "/s"
    if resource_delta > 0:
        delta_text = "+" + delta_text
    delta_label.text = delta_text
