<template>
    <ComChip v-if="gv.promotion && gv.promotion.length == 1" tooltip="Happy Hour Promotion" prepend-icon="mdi-tag-multiple">{{ gv.promotion[0].promotion_name }}</ComChip>
    <template v-else-if="gv.promotion && gv.promotion.length > 1">
        <v-badge :content="gv.promotion?.length" color="orange">
            <ComChip @onClick="onClick()" tooltip="Happy Hour Promotions" prepend-icon="mdi-tag-multiple">Happy Hour Promotions</ComChip>
        </v-badge>
    </template>
</template>
<script setup>
    import {inject, viewHappyHourPromotionModal} from '@/plugin'
    const gv = inject('$gv')
    function onClick(){
        const r = viewHappyHourPromotionModal({})
    }
</script> 