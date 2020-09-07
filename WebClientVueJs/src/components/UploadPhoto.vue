<template>
    <div>
        <label>
            Upload photos:
            <input type="file" id="files" ref="files" multiple v-on:change="handleFilesUpload()" />
        </label>
        <button v-on:click="submitFiles()">Send</button>
    </div>
</template>

<script>
    import Axios from 'axios';
    import Config from '@/config';
    import '@/cookies';
    import Utils from '@/utils';

    export default {
        data() {
            return {
                files: []
            }
        },
        methods: {
            handleFilesUpload() {
                this.files = this.$refs.files.files;
            },
            submitFiles() {
                var self = this;
                var uploadFunc = function (filesBase64) {
                    Axios({
                        method: 'post',
                        url: Config.uploadPhotosApiPath,
                        data: { login: self.$cookies.get('login'), token: self.$cookies.get('token'), files: filesBase64 }
                    }).then(response => {
                        if (response.data.isAuthenticated == false) {
                            this.$router.push('/');
                        } else if (response.data.success) {
                            self.submitFilesCompleted(response.data.photoIds);
                        }
                    });
                };
                Utils.filesToBase64(this.files, uploadFunc);
            },
            submitFilesCompleted(photoIds) {
                this.$emit('onUpload', photoIds);
            }
        }
    }
</script>

<style scoped>
</style>
