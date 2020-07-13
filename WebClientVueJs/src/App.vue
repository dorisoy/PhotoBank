<template>
    <div id="app">
        <router-view />
    </div>
</template>

<script>
    import Auth from '@/components/Auth.vue';

    export default {
        name: 'app',
        components: {
            Auth
        }
    };
</script>

<style>
</style>
