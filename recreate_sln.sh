name="$1"; path=""

# https://www.diskinternals.com/linux-reader/bash-string-ends-with/
if [[ "$name" == *.sln ]]
# https://stackoverflow.com/a/16623897
then path="$name"; name=${1%".sln"}
else path="${name}.sln"
fi

rm "$path"
dotnet new sln --name $name

# https://stackoverflow.com/a/5247919
# https://unix.stackexchange.com/a/34328
find . \( -name '*.csproj' \) -print0 | sort -z | xargs -0 dotnet sln $path add