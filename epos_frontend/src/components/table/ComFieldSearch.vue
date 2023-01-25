<template>
    <v-sheet v-click-outside="onOutsideClick" class="relative form-label">
        <label class="block">
            {{ customers.data }}
            <div class="mb-2 text-sm leading-4">
                {{ label }} <span v-if="validate" class="text-validate">*</span>
            </div>
            <input type="text" v-model="selected" class="form-input" @focus="onInput(true)" @input="onInput(true)" :required="validate"/>
        </label>
        <div v-show="open">
            <ul
                class="rounded-[6px] bg-white h-auto shadow-md z-50 px-[7px] py-[6px] absolute w-full max-h-28 overflow-auto mt-1">
                <li class="flex flex-row items-center text-base font-normal cursor-pointer px-[13px] py-[5px] rounded-[8px] space-x-[10px] text-gray-800 hover:bg-gray-100"
                    v-for="item in customers.data" :key="item" :value="item" @click="onSelected(item)" :class="{
                        'bg-gray-100': item === selected
                    }">
                    {{ item.name }} - {{ item.customer_name_en }}
                </li>
                <li v-if="(customers.data && customers.data.length === 0)"
                    class="flex flex-row italic items-center cursor-not-allowed text-sm font-normal px-[13px] py-[5px] rounded-[8px] space-x-[10px] text-gray-500">
                    Not Found
                </li>
            </ul>
        </div>
    </v-sheet>
</template>
<script>

import { ref } from 'vue'
import {createResource} from '@/resource.js'
export default {
    props: {
        modelValue: String,
        label: String,
        validate: Boolean
    },
    data() {
        return {
            customers: [],
            selected: ref(''),
            searching: false,
            open: false
        }
    },
    watch: {
        selected(newValue) {
            this.onSearch(newValue)
        },
    },
    methods: {
        async onSearch(keyword) {
            if (keyword) {
                this.seaching = true;
                this.customers = createResource({
                    url: "frappe.client.get_list",
                    params: {
                        doctype: 'Customer',
                        fields: ["name","customer_name_en"],
                        filters: {'customer_name_en':["like",'%'+ keyword + '%']},
                        debounce: 1500,
                    },
                    auto:true 
                });
            }else{
                this.customers = createResource({
                    url: "frappe.client.get_list",
                    params: {
                        doctype: 'Customer',
                        fields: ["name","customer_name_en"],
                    },
                    auto:true 
                });
            }
        },
        onSelected(item) {
            this.selected = item.name;
            this.$emit('update:modelValue', item.name)
            this.onInput(false);
        },
        onOutsideClick() {
            if (this.customers.length > 0 && !this.customers.find((r) => r.name === this.selected)) {
                this.selected = '';
            }
            this.$emit('update:modelValue', this.selected);
            this.onInput(false);
        },
        onInput($event) {
            this.open = $event;
        },
    },

    async mounted() {
        await this.$nextTick(function () {
            if (this.modelValue) {
                this.selected = this.modelValue;
            }

        });
    }
}    
</script>