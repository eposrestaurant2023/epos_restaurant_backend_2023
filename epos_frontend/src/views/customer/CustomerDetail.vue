<template>
    <h1>Customer Detail</h1>
    <div>{{ customer.doc }}</div>
  </template>
  <script setup>
  import { useRoute, createDocumentResource } from '@/plugin'
  const route = useRoute()

    let customer = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Customer',
    name: route.params.name,
    auto:true
    })
  </script>