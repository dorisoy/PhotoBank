import Vue from 'vue';
import Router from 'vue-router';
import Auth from '@/components/Auth.vue';
import Photos from '@/components/Photos.vue';

Vue.use(Router);

export default new Router({
    mode: 'history',
    routes: [
        { path: '/', component: Auth },
        { path: '/photos', component: Photos }
    ]
});
