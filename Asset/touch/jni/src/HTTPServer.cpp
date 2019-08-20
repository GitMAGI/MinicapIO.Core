/**
 *                  _           _     _   _                   _
 *  __ _ _ __   __| |_ __ ___ (_) __| | | |_ ___  _   _  ___| |__
 * / _` | '_ \ / _` | '__/ _ \| |/ _` | | __/ _ \| | | |/ __| '_ \
 *| (_| | | | | (_| | | | (_) | | (_| | | || (_) | |_| | (__| | | |
 * \__,_|_| |_|\__,_|_|  \___/|_|\__,_|  \__\___/ \__,_|\___|_| |_|
 *
 * Copyright (c) 2017 Kunal Dawn <kunal.dawn@gmail.com>
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 3 as published by
 * the Free Software Foundation.
 */

#include "HTTPServer.h"

int main() {
    android_touch::Logging::setMode(android_touch::Logging::Mode::Info);
    
    android_touch::Logging::debug("Server", "Main Starting ...");    
    android_touch::HTTPServer httpServer("0.0.0.0", 9889);    
    android_touch::Logging::debug("Server", "Instance Created!");
        
    httpServer.run();
    
    android_touch::Logging::debug("Server", "Main Completed!");
}

android_touch::HTTPServer::HTTPServer(const std::string &host, int port) {
    mHost = host;
    mPort = port;
}

void android_touch::HTTPServer::run() {
    
    auto touchInput = TouchInput::getNewInstance();

    if (touchInput == nullptr) {
        Logging::info("Server", "No supported touch input found...");
        return;
    }

    Logging::info("Server", "Using input device : " + touchInput->getDevicePath());

    mServer.post("/", [&touchInput, this](const httplib::Request& req, httplib::Response& res) {
        Logging::debug("Server", "Request Method: " + req.method);        
        for(std::pair<std::string, std::string> header : req.headers){
            std::string key = header.first;
            std::string value = header.second;
            Logging::debug("Server", "Request Header: " + key + " → " + value);
        }
        Logging::debug("Server", "Request Body: " + req.body);
        try {
            Json::Value root;
            Json::Reader reader;
            reader.parse(req.body, root, false);

            for (Json::Value &command : root) {
                Logging::debug("Server", "Deserializing a Request ...");
                std::vector<std::string> members = command.getMemberNames();
                for(std::string &member : members){
                    std::string key = member;
                    std::string value = command.get(key, Json::stringValue).asString();
                    Logging::debug("Server", key + ": " + value);
                }
            }

            for (Json::Value &command : root) {
                if (command.isMember("type")) {
                    std::string commandType = command.get("type", Json::stringValue).asString();
                    if (commandType == "down" && command.isMember("x") && command.isMember("y") && command.isMember("contact") && command.isMember("pressure")) {
                        int x = command.get("x", Json::intValue).asInt();
                        int y = command.get("y", Json::intValue).asInt();
                        int contact = command.get("contact", Json::intValue).asInt();
                        int pressure = command.get("pressure", Json::intValue).asInt();

                        touchInput->down(contact, x, y, pressure);
                    } else if (commandType == "move" && command.isMember("x") && command.isMember("y") && command.isMember("contact") && command.isMember("pressure")) {
                        int x = command.get("x", Json::intValue).asInt();
                        int y = command.get("y", Json::intValue).asInt();
                        int contact = command.get("contact", Json::intValue).asInt();
                        int pressure = command.get("pressure", Json::intValue).asInt();

                        touchInput->move(contact, x, y, pressure);
                    } else if (commandType == "up" && command.isMember("contact")) {
                        int contact = command.get("contact", Json::intValue).asInt();

                        touchInput->up(contact);
                    } else if (commandType == "commit") {
                        touchInput->commit();
                    } else if (commandType == "delay" && command.isMember("value")) {
                        int value = command.get("value", Json::intValue).asInt();

                        touchInput->delay(value);
                    } else if (commandType == "reset") {
                        touchInput->reset();
                    } else if (commandType == "stop") {
                        Logging::info("Server", "good bye cruel world!");
                        mServer.stop();
                    }
                }
            }
        } catch (std::exception &ex) {
            std::cout << ex.what() << std::endl;
        }
    });

    Logging::info("Server", "Starting server on port : " + std::to_string(mPort));
    mServer.listen(mHost.c_str(), mPort);
}
