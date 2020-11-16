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
        mounted() {
            var self = this;
            self.clientId = Utils.getClientId();
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
                        data: { clientId: self.clientId, login: self.$cookies.get('login'), token: self.$cookies.get('token'), files: filesBase64 }
                    });
                };
                Utils.filesToBase64(this.files, uploadFunc);
            }
        }
    }
</script>

<style scoped>
</style>
