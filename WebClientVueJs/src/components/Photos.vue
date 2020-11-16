<template>
    <div id="photos">
        <h1>Photos</h1>
        <uploadPhoto />
        <div>
            <ul>
                <li v-for="(photo, i) in photos" v-bind:key="i">
                    <img v-bind:src="photo" width="250" />
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
    import Axios from 'axios';
    import Config from '@/config';
    import '@/cookies';
    import SignalR from '@/signalr';
    import UploadPhoto from '@/components/UploadPhoto.vue';
    import Utils from '@/utils';

    export default {
        name: 'Photos',
        components: {
            'uploadPhoto': UploadPhoto
        },
        data() {
            return {
                photos: []
            }
        },
        mounted() {
            var self = this;
            self.clientId = Utils.getClientId();
            SignalR.start(self.clientId);
            SignalR.connection.on("GetPhotosResponse", function (response) {
                if (response.success) {
                    if (response.isAuthenticated == false) {
                        this.$router.push('/');
                    } else if (response.success) {
                        self.loadPhotosContent(response.photoIds);
                    }
                }
            });
            SignalR.connection.on("GetPhotoResponse", function (response) {
                if (response.success) {
                    self.photos.push('data:image/png;base64,' + response.fileBase64Content);
                }
            });
            SignalR.connection.on("UploadPhotosResponse", function (response) {
                if (response.success) {
                    self.loadPhotosContent([response.photoId]);
                }
            });
            // получаем список id всех фоток
            Axios({
                method: 'post',
                url: Config.getPhotosApiPath,
                data: { clientId: this.clientId, login: this.$cookies.get('login'), token: this.$cookies.get('token') }
            });
        },
        methods: {
            loadPhotosContent(photoIds) {
                for (var photoIdIndex in photoIds) {
                    // получаем содержимое каждой фотки
                    Axios({
                        method: 'post',
                        url: Config.getPhotoApiPath,
                        data: { clientId: this.clientId, login: this.$cookies.get('login'), token: this.$cookies.get('token'), photoId: photoIds[photoIdIndex] }
                    });
                }
            }
        }
    };
</script>

<style scoped>
    li {
        display: inline-block;
    }
</style>
