<template>
    <div id="photos">
        <h1>Photos</h1>
        <uploadPhoto v-on:onUpload="onUpload" />
        <ul>
            <li v-for="photoUrl in photoUrls" v-bind:key="photoUrl">
                <img v-bind:src="photoUrl" width="250" />
            </li>
        </ul>
    </div>
</template>

<script>
    import Axios from 'axios';
    import Config from '@/config';
    import '@/cookies';
    import UploadPhoto from '@/components/UploadPhoto.vue';

    export default {
        name: 'Photos',
        components: {
            'uploadPhoto': UploadPhoto
        },
        data() {
            return {
                photoUrls: []
            }
        },
        mounted() {
            // получаем список id всех фоток
            Axios({
                method: 'post',
                url: Config.getPhotosApiPath,
                data: { login: this.$cookies.get('login'), token: this.$cookies.get('token') }
            }).then(response => {
                if (response.data.isAuthenticated == false) {
                    this.$router.push('/');
                } else if (response.data.success) {
                    var photoIds = response.data.photoIds;
                    this.loadPhotosAndAddToPhotoUrls(photoIds);
                }
            });
        },
        methods: {
            onUpload(photoIds) {
                this.loadPhotosAndAddToPhotoUrls(photoIds);
            },
            loadPhotosAndAddToPhotoUrls(photoIds) {
                for (var photoIdIndex in photoIds) {
                    // формируем адрес для получения содержимого каждой фотки
                    var photoUrl = Config.getPhotoApiPath + '?login=' + this.$cookies.get('login') + '&token=' + this.$cookies.get('token') + '&photoId=' + photoIds[photoIdIndex];
                    this.photoUrls.push(photoUrl);
                }
            }
        }
    };
</script>

<style scoped>
</style>
