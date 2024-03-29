﻿    document.addEventListener('DOMContentLoaded', function () {
        function get(url) {
            return new Promise((resolve, reject) => {
                var xhr = new XMLHttpRequest();
                xhr.open('GET', url, true);
                xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
                xhr.send();
                xhr.onload = () => {
                    if (xhr.status >= 200 && xhr.status < 300) {
                        resolve(xhr.response || xhr.responseText);
                    } else {
                        reject({ message: xhr.statusText, statusCode: xhr.status });
                    }
                }
                xhr.onerror = () => {
                    reject({ message: xhr.statusText, statusCode: xhr.status });
                }
            });
        }

            function generateRandomName() {
                return Math.random().toString(36).substring(2, 10);
            }

            // Get the user name and store it to prepend to messages.
            var authType = "cookie";
            do {
        authType = prompt("Choose auth type: cookie / jwt", authType);
                if (authType != "cookie" && authType != "jwt") {
        authType = '';
                }
            } while (!authType)

            var username = `${authType}-` + generateRandomName();
            var promptMessage = `Enter your name (Only starting with ${authType} can get auth):`;
            var role = "Admin";
            var promptRoleMessage = 'Enter your role (Only Admin can get auth)';
            do {
        username = prompt(promptMessage, username);
                if (!username || username.startsWith('_') || username.indexOf('<') > -1 || username.indexOf('>') > -1) {
        username = '';
                    promptMessage = 'Invalid input. Enter your name:';
                } else {
        role = prompt(promptRoleMessage, role);
                }
            } while (!username && !role)

            // Set initial focus to message input box.
            var messageInput = document.getElementById('message');
            messageInput.focus();
            var groupOperationInput = document.getElementById('groupoperationname');
            var groupInput = document.getElementById('groupname');
            var groupExceptInput = document.getElementById('groupexcept');
            var userInput = document.getElementById('username');
            var restapiInput = document.getElementById('restapirequest');

            function createMessageEntry(encodedName, encodedMsg) {
                var entry = document.createElement('div');
                entry.classList.add("message-entry");
                if (encodedName === "_SYSTEM_") {
        entry.innerHTML = encodedMsg;
                    entry.classList.add("text-center");
                    entry.classList.add("system-message");
                } else if (encodedName === "_BROADCAST_") {
        entry.classList.add("text-center");
                    entry.innerHTML = `<div class="text-center broadcast-message">${encodedMsg}</div>`;
                } else if (encodedName === username) {
        entry.innerHTML = `<div class="message-avatar pull-right">${encodedName}</div>` +
        `<div class="message-content pull-right">${encodedMsg}<div>`;
                } else {
        entry.innerHTML = `<div class="message-avatar pull-left">${encodedName}</div>` +
        `<div class="message-content pull-left">${encodedMsg}<div>`;
                }
                return entry;
            }

            function appendMessage(encodedName, encodedMsg) {
                var messageEntry = createMessageEntry(encodedName, encodedMsg);
                var messageBox = document.getElementById('messages');
                messageBox.appendChild(messageEntry);
                messageBox.scrollTop = messageBox.scrollHeight;
            }

            function bindConnectionMessage(connection) {
                var messageCallback = function (name, message) {
                    if (!message) return;
                    // Html encode display name and message.
                    var encodedName = name;
                    var encodedMsg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
appendMessage(encodedName, encodedMsg);
                };
// Create a function that the hub can call to broadcast messages.
connection.on('broadcastMessage', messageCallback);
connection.on('echo', messageCallback);
connection.onclose(onConnectionError);
            }

function onConnected(connection) {
    console.log('connection started');
    connection.send('broadcastMessage', '_SYSTEM_', username + ' JOINED');
    document.getElementById('sendmessage').addEventListener('click', function (event) {
        // Call the broadcastMessage method on the hub.
        if (messageInput.value) {
            connection.send('broadcastMessage', username, messageInput.value);
        }

        // Clear text box and reset focus for next comment.
        messageInput.value = '';
        messageInput.focus();
        event.preventDefault();
    });
    document.getElementById('message').addEventListener('keypress', function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            document.getElementById('sendmessage').click();
            return false;
        }
    });
    document.getElementById('echo').addEventListener('click', function (event) {
        // Call the echo method on the hub.
        connection.send('echo', username, messageInput.value);

        // Clear text box and reset focus for next comment.
        messageInput.value = '';
        messageInput.focus();
        event.preventDefault();
    });

    // Group join/leave operations
    document.getElementById('joingroup').addEventListener('click', function (event) {
        if (groupOperationInput.value) {
            connection.send('joingroup', username, groupOperationInput.value);
        }

        groupOperationInput.value = '';
        groupOperationInput.focus();
        event.preventDefault();
    });
    document.getElementById('leavegroup').addEventListener('click', function (event) {
        if (groupOperationInput.value) {
            connection.send('leavegroup', username, groupOperationInput.value);
        }

        groupOperationInput.value = '';
        groupOperationInput.focus();
        event.preventDefault();
    });

    // Send to Group/Groups
    document.getElementById('sendgroup').addEventListener('click', function (event) {
        if (groupInput.value && messageInput.value) {
            connection.send('sendgroup', username, groupInput.value, messageInput.value);
        }

        messageInput.value = '';
        messageInput.focus();
        event.preventDefault();
    });
    document.getElementById('sendgroups').addEventListener('click', function (event) {
        if (groupInput.value && messageInput.value) {
            connection.send('sendgroups', username, groupInput.value.split(","), messageInput.value);
        }

        messageInput.value = '';
        messageInput.focus();
        event.preventDefault();
    });
    document.getElementById('sendgroupexcept').addEventListener('click', function (event) {
        if (groupInput.value && messageInput.value && groupExceptInput.value) {
            connection.send('sendgroupexcept', username, groupInput.value, groupExceptInput.value.split(','), messageInput.value);
        }

        messageInput.value = '';
        messageInput.focus();
        event.preventDefault();
    });

    // Send to User/Users
    document.getElementById('senduser').addEventListener('click', function (event) {
        if (userInput.value && messageInput.value) {
            connection.send('senduser', username, userInput.value, messageInput.value);
        }

        messageInput.value = '';
        messageInput.focus();
        event.preventDefault();
    });
    document.getElementById('sendusers').addEventListener('click', function (event) {
        if (userInput.value && messageInput.value) {
            connection.send('sendusers', username, userInput.value.split(","), messageInput.value);
        }

        messageInput.value = '';
        messageInput.focus();
        event.preventDefault();
    });
}

function onConnectionError(error) {
    if (error && error.message) {
        console.error(error.message);
    }
    var modal = document.getElementById('myModal');
    modal.classList.add('in');
    modal.style = 'display: block;';
}

var jwtToken = '';
var connection = new signalR.HubConnectionBuilder()
    .withUrl(`/chat${authType}?username=${username}`,
        {
            accessTokenFactory: () => jwtToken
        })
    .build();

var url = `/${authType}/login?username=${username}&role=${role}`;
get(url).then((token) => {
    jwtToken = token;
    bindConnectionMessage(connection);
    return connection.start();
})
    .then(function () {
        onConnected(connection);
    })
    .catch(function (error) {
        if (error.message) {
            console.error(error.message);
        }

        if (error.statusCode && error.statusCode === 401) {
            appendMessage('_SYSTEM_', 'You\'re not logged in. Refresh and choose a valid username to login');
        }

        if (error.statusCode && error.statusCode === 403) {
            appendMessage('_SYSTEM_', 'The role cannot be authrized. Refresh and choose Admin to login');
        }
    });
        });
