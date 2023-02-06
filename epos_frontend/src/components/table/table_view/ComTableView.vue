<template>
    <div>
        <v-table fixed-header>
            <thead>
                <slot name="header"></slot>
            </thead>
            <tbody>
                <slot name="body"></slot>
            </tbody>
            <tfoot>
                <slot name="footer"></slot>
            </tfoot>
        </v-table>
    </div>
</template>
<script>
export default {
    
}
</script>
<style>
.view_table {
    border-collapse: collapse;
    border-spacing: 0;
    width: 100%;
    margin-bottom: 20px;
}
.view_table, .view_table th, .view_table td {
    border: 1px solid #e3e3e3;
    padding: 8px 12px;
}
.view_table th {
    background: #ebeff3;
    color: #041a15;
    text-transform: capitalize;
    font-weight: 400;
    width: 1px;
    padding: 16px 12px !important;
    font-size: 14px;
}
</style>
